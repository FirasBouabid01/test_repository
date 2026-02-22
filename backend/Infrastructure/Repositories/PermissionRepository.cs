using Domain.Entities;
using Application.Interfaces;
using Application.Common.Pagination;
using Application.Permissions.Dtos;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
{
    private readonly AppDbContext _context;

    public PermissionRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Permission?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<IEnumerable<Permission>> GetAllWithDetailsAsync()
    {
        return await _context.Permissions
            .Include(p => p.RolePermissions)
                .ThenInclude(rp => rp.Role)
            .Include(p => p.UserPermissions)
                .ThenInclude(up => up.User)
            .ToListAsync();
    }

    public async Task<PaginatedResponse<PermissionDto>> GetPaginatedPermissionsAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        string? sortBy = "name",
        bool sortDescending = false)
    {
        var query = _context.Permissions
            .Include(p => p.RolePermissions)
            .Include(p => p.UserPermissions)
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrEmpty(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(term));
        }

        // Apply sorting
        query = sortBy?.ToLower() switch
        {
            _ => sortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
        };

        var totalCount = await query.CountAsync();
        var skip = (pageNumber - 1) * pageSize;
        var permissions = await query
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        var permissionDtos = permissions.Select(p => new PermissionDto
        {
            Id = p.Id,
            Name = p.Name,
            RoleCount = p.RolePermissions.Count,
            UserCount = p.UserPermissions.Count
        }).ToList();

        return new PaginatedResponse<PermissionDto>(permissionDtos, pageNumber, pageSize, totalCount);
    }
}
