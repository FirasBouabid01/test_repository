using Application.TrainingLevels.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.TrainingLevels.Commands.CreateTrainingLevel;

public record CreateTrainingLevelCommand(
    string Name,
    string? Description,
    Guid? PrerequisiteLevelId) : IRequest<TrainingLevelDto>;

public class CreateTrainingLevelCommandHandler : IRequestHandler<CreateTrainingLevelCommand, TrainingLevelDto>
{
    private readonly ITrainingLevelRepository _repository;

    public CreateTrainingLevelCommandHandler(ITrainingLevelRepository repository)
    {
        _repository = repository;
    }

    public async Task<TrainingLevelDto> Handle(CreateTrainingLevelCommand request, CancellationToken cancellationToken)
    {
        var maxSequence = await _repository.GetMaxSequenceOrderAsync();

        var trainingLevel = new TrainingLevel
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            PrerequisiteLevelId = request.PrerequisiteLevelId,
            SequenceOrder = maxSequence + 1,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(trainingLevel);

        return new TrainingLevelDto
        {
            Id = trainingLevel.Id,
            Name = trainingLevel.Name,
            Description = trainingLevel.Description,
            SequenceOrder = trainingLevel.SequenceOrder,
            PrerequisiteLevelId = trainingLevel.PrerequisiteLevelId,
            IsActive = trainingLevel.IsActive,
            CreatedAt = trainingLevel.CreatedAt,
            UpdatedAt = trainingLevel.UpdatedAt
        };
    }
}
