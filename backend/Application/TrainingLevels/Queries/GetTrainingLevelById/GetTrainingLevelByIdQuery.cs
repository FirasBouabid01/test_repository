using Application.TrainingLevels.Dtos;
using Domain.Interfaces;
using MediatR;

namespace Application.TrainingLevels.Queries.GetTrainingLevelById;

public record GetTrainingLevelByIdQuery(Guid Id) : IRequest<TrainingLevelDto?>;

public class GetTrainingLevelByIdQueryHandler : IRequestHandler<GetTrainingLevelByIdQuery, TrainingLevelDto?>
{
    private readonly ITrainingLevelRepository _repository;

    public GetTrainingLevelByIdQueryHandler(ITrainingLevelRepository repository)
    {
        _repository = repository;
    }

    public async Task<TrainingLevelDto?> Handle(GetTrainingLevelByIdQuery request, CancellationToken cancellationToken)
    {
        var level = await _repository.GetByIdAsync(request.Id);
        
        if (level == null) return null;

        // Since GetByIdAsync doesn't include navigation properties by default in some generic repos,
        // and we need stats, we might want a specialized repository method or a more robust GetById.
        // However, looking at TrainingLevelRepository, we can use GetLevelWithStatsAsync or similar.
        
        // Actually, let's just fetch it with necessary includes if possible.
        // For now, I'll use the repository's stats method if I added one.
        
        // Let's check what I implemented in repository earlier.
        // I implemented GetLevelWithStatsAsync(Guid id) which returns object?. 
        // That's not ideal for MediatR DTO.
        
        // I'll update ITrainingLevelRepository to have a proper DTO-friendly GetByIdWithStatsAsync.
        
        // Wait, I already have GetAllWithStatsAsync. I can just filter it here or add a specific method.
        var allLevels = await _repository.GetAllWithStatsAsync();
        var target = allLevels.FirstOrDefault(l => l.Id == request.Id);
        
        if (target == null) return null;

        return new TrainingLevelDto
        {
            Id = target.Id,
            Name = target.Name,
            Description = target.Description,
            SequenceOrder = target.SequenceOrder,
            PrerequisiteLevelId = target.PrerequisiteLevelId,
            PrerequisiteLevelName = target.PrerequisiteLevel?.Name,
            LeaderCount = target.LeaderProgresses.Count,
            TaskCount = 0,
            IsActive = target.IsActive,
            CreatedAt = target.CreatedAt,
            UpdatedAt = target.UpdatedAt
        };
    }
}
