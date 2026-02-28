using Application.Common.Models;
using Application.TrainingLevels.Dtos;
using Domain.Interfaces;
using MediatR;

namespace Application.TrainingLevels.Queries.GetAllTrainingLevels;

public record GetAllTrainingLevelsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    bool? IsActive = null) : IRequest<PaginatedList<TrainingLevelDto>>;

public class GetAllTrainingLevelsQueryHandler : IRequestHandler<GetAllTrainingLevelsQuery, PaginatedList<TrainingLevelDto>>
{
    private readonly ITrainingLevelRepository _repository;

    public GetAllTrainingLevelsQueryHandler(ITrainingLevelRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedList<TrainingLevelDto>> Handle(GetAllTrainingLevelsQuery request, CancellationToken cancellationToken)
    {
        var levels = await _repository.GetAllWithStatsAsync(request.IsActive);

        var dtos = levels.Select(l => new TrainingLevelDto
        {
            Id = l.Id,
            Name = l.Name,
            Description = l.Description,
            SequenceOrder = l.SequenceOrder,
            PrerequisiteLevelId = l.PrerequisiteLevelId,
            PrerequisiteLevelName = l.PrerequisiteLevel?.Name,
            LeaderCount = l.LeaderProgresses.Count,
            TaskCount = 0, // Placeholder as TrainingTask entity is not implemented yet
            IsActive = l.IsActive,
            CreatedAt = l.CreatedAt,
            UpdatedAt = l.UpdatedAt
        }).ToList();

        var paginatedList = new PaginatedList<TrainingLevelDto>(
            dtos.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList(),
            dtos.Count,
            request.PageNumber,
            request.PageSize
        );

        return paginatedList;
    }
}
