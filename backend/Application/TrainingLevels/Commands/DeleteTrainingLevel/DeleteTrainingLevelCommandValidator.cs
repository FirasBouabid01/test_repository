using Domain.Interfaces;
using FluentValidation;

namespace Application.TrainingLevels.Commands.DeleteTrainingLevel;

public class DeleteTrainingLevelCommandValidator : AbstractValidator<DeleteTrainingLevelCommand>
{
    private readonly ITrainingLevelRepository _repository;

    public DeleteTrainingLevelCommandValidator(ITrainingLevelRepository repository)
    {
        _repository = repository;

        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Id is required.")
            .MustAsync(Exist).WithMessage("Training level not found.")
            .MustAsync(NotHaveDependentLevels).WithMessage("Cannot delete a level that is a prerequisite for other levels.")
            .MustAsync(NotHaveAssignedLeaders).WithMessage("Cannot delete a level with assigned leaders.");
    }

    private async Task<bool> Exist(Guid id, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(id) != null;
    }

    private async Task<bool> NotHaveDependentLevels(Guid id, CancellationToken cancellationToken)
    {
        return !await _repository.HasDependentLevelsAsync(id);
    }

    private async Task<bool> NotHaveAssignedLeaders(Guid id, CancellationToken cancellationToken)
    {
        return !await _repository.HasAssignedLeadersAsync(id);
    }
}
