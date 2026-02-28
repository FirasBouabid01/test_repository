using Application.TrainingLevels.Dtos;
using Domain.Interfaces;
using MediatR;

namespace Application.TrainingLevels.Commands.ReorderLevels;

public record ReorderLevelsCommand(List<ReorderLevelsDto> Levels) : IRequest<IEnumerable<TrainingLevelDto>>;

public class ReorderLevelsCommandHandler : IRequestHandler<ReorderLevelsCommand, IEnumerable<TrainingLevelDto>>
{
    private readonly ITrainingLevelRepository _repository;

    public ReorderLevelsCommandHandler(ITrainingLevelRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TrainingLevelDto>> Handle(ReorderLevelsCommand request, CancellationToken cancellationToken)
    {
        var results = new List<TrainingLevelDto>();

        foreach (var levelDto in request.Levels)
        {
            var level = await _repository.GetByIdAsync(levelDto.Id);
            if (level != null)
            {
                level.SequenceOrder = levelDto.SequenceOrder;
                level.UpdatedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(level);
                
                results.Add(new TrainingLevelDto
                {
                    Id = level.Id,
                    Name = level.Name,
                    SequenceOrder = level.SequenceOrder,
                    UpdatedAt = level.UpdatedAt
                });
            }
        }

        return results;
    }
}
