using Application.TrainingLevels.Dtos;
using Domain.Interfaces;
using MediatR;

namespace Application.TrainingLevels.Queries.GetLevelHierarchy;

public record GetLevelHierarchyQuery() : IRequest<IEnumerable<TrainingLevelHierarchyDto>>;

public class GetLevelHierarchyQueryHandler : IRequestHandler<GetLevelHierarchyQuery, IEnumerable<TrainingLevelHierarchyDto>>
{
    private readonly ITrainingLevelRepository _repository;

    public GetLevelHierarchyQueryHandler(ITrainingLevelRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TrainingLevelHierarchyDto>> Handle(GetLevelHierarchyQuery request, CancellationToken cancellationToken)
    {
        var levels = await _repository.GetAllWithStatsAsync();
        
        // Build the tree
        var allDtos = levels.Select(l => new TrainingLevelHierarchyDto
        {
            Id = l.Id,
            Name = l.Name,
            SequenceOrder = l.SequenceOrder,
            Children = new List<TrainingLevelHierarchyDto>()
        }).ToList();

        var rootLevels = new List<TrainingLevelHierarchyDto>();
        var dtoDict = allDtos.ToDictionary(d => d.Id);

        foreach (var l in levels)
        {
            var dto = dtoDict[l.Id];
            if (l.PrerequisiteLevelId == null)
            {
                rootLevels.Add(dto);
            }
            else if (dtoDict.ContainsKey(l.PrerequisiteLevelId.Value))
            {
                dtoDict[l.PrerequisiteLevelId.Value].Children.Add(dto);
            }
            else
            {
                // Prerequisite not in the list (should not happen with GetAllWithStatsAsync)
                rootLevels.Add(dto);
            }
        }

        return rootLevels.OrderBy(r => r.SequenceOrder);
    }
}
