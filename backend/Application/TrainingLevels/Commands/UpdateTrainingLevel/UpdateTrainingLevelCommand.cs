using Application.TrainingLevels.Dtos;
using Domain.Interfaces;
using MediatR;

namespace Application.TrainingLevels.Commands.UpdateTrainingLevel;

public record UpdateTrainingLevelCommand(
    Guid Id,
    string Name,
    string? Description,
    Guid? PrerequisiteLevelId,
    bool IsActive) : IRequest<TrainingLevelDto>;

public class UpdateTrainingLevelCommandHandler : IRequestHandler<UpdateTrainingLevelCommand, TrainingLevelDto>
{
    private readonly ITrainingLevelRepository _repository;

    public UpdateTrainingLevelCommandHandler(ITrainingLevelRepository repository)
    {
        _repository = repository;
    }

    public async Task<TrainingLevelDto> Handle(UpdateTrainingLevelCommand request, CancellationToken cancellationToken)
    {
        var trainingLevel = await _repository.GetByIdAsync(request.Id);
        
        if (trainingLevel == null) throw new Exception("Training level not found");

        trainingLevel.Name = request.Name;
        trainingLevel.Description = request.Description;
        trainingLevel.PrerequisiteLevelId = request.PrerequisiteLevelId;
        trainingLevel.IsActive = request.IsActive;
        trainingLevel.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(trainingLevel);

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
