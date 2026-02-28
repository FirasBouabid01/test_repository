using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of the training level repository.
/// </summary>
public class TrainingLevelRepository : GenericRepository<TrainingLevel>, ITrainingLevelRepository
{
    private readonly AppDbContext _context;

    public TrainingLevelRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TrainingLevel>> GetAllOrderedAsync(int pageNumber, int pageSize)
    {
        return await _context.TrainingLevels
            .OrderBy(tl => tl.SequenceOrder)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(tl => tl.PrerequisiteLevel)
            .ToListAsync();
    }

    public async Task<IEnumerable<TrainingLevel>> GetAllWithStatsAsync(bool? isActive = null)
    {
        IQueryable<TrainingLevel> query = _context.TrainingLevels
            .Include(tl => tl.PrerequisiteLevel)
            .Include(tl => tl.LeaderProgresses);

        if (isActive.HasValue)
        {
            query = query.Where(tl => tl.IsActive == isActive.Value);
        }

        return await query.OrderBy(tl => tl.SequenceOrder).ToListAsync();
    }

    public async Task<TrainingLevel?> GetByNameAsync(string name)
    {
        return await _context.TrainingLevels
            .FirstOrDefaultAsync(tl => tl.Name.ToLower() == name.ToLower());
    }

    public async Task<bool> HasAssignedLeadersAsync(Guid levelId)
    {
        return await _context.LeaderProgresses.AnyAsync(lp => lp.CurrentLevelId == levelId);
    }

    public async Task<bool> HasDependentLevelsAsync(Guid levelId)
    {
        return await _context.TrainingLevels
            .AnyAsync(tl => tl.PrerequisiteLevelId == levelId);
    }

    public async Task<bool> IsCircularDependencyAsync(Guid levelId, Guid? prerequisiteId)
    {
        if (prerequisiteId == null) return false;
        if (levelId == prerequisiteId) return true;

        var currentPrereqId = prerequisiteId;
        
        // Walk up the chain (recursive check in iterative way to prevent stack overflow)
        // Note: In a real world scenario with many levels, we might need a more optimized approach,
        // but for training levels, the chain is usually short (3-10 levels).
        var visited = new HashSet<Guid> { levelId };
        
        while (currentPrereqId != null)
        {
            if (visited.Contains(currentPrereqId.Value)) return true;
            visited.Add(currentPrereqId.Value);

            var level = await _context.TrainingLevels
                .AsNoTracking()
                .Select(tl => new { tl.Id, tl.PrerequisiteLevelId })
                .FirstOrDefaultAsync(tl => tl.Id == currentPrereqId);

            if (level == null) break;
            currentPrereqId = level.PrerequisiteLevelId;
        }

        return false;
    }

    public async Task<int> GetMaxSequenceOrderAsync()
    {
        if (!await _context.TrainingLevels.AnyAsync()) return 0;
        return await _context.TrainingLevels.MaxAsync(tl => tl.SequenceOrder);
    }

    public async Task<object?> GetLevelWithStatsAsync(Guid id)
    {
        var level = await _context.TrainingLevels
            .Include(tl => tl.PrerequisiteLevel)
            .FirstOrDefaultAsync(tl => tl.Id == id);

        if (level == null) return null;

        var userCount = await _context.LeaderProgresses
            .CountAsync(lp => lp.CurrentLevelId == id);

        return new
        {
            level.Id,
            level.Name,
            level.Description,
            level.SequenceOrder,
            PrerequisiteName = level.PrerequisiteLevel?.Name,
            UserCount = userCount,
            level.IsActive,
            level.CreatedAt,
            level.UpdatedAt
        };
    }
}
