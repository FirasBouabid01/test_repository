using Domain.Interfaces;
using MediatR;

namespace Application.TrainingLevels.Commands.DeleteTrainingLevel;

public record DeleteTrainingLevelCommand(Guid Id) : IRequest<bool>;

public class DeleteTrainingLevelCommandHandler : IRequestHandler<DeleteTrainingLevelCommand, bool>
{
    private readonly ITrainingLevelRepository _repository;

    public DeleteTrainingLevelCommandHandler(ITrainingLevelRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteTrainingLevelCommand request, CancellationToken cancellationToken)
    {
        var trainingLevel = await _repository.GetByIdAsync(request.Id);
        
        if (trainingLevel == null) return false;

        trainingLevel.IsActive = false;
        trainingLevel.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(trainingLevel);
        return true;
    }
}
