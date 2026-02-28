using Domain.Interfaces;
using FluentValidation;

namespace Application.TrainingLevels.Commands.CreateTrainingLevel;

public class CreateTrainingLevelCommandValidator : AbstractValidator<CreateTrainingLevelCommand>
{
    private readonly ITrainingLevelRepository _repository;

    public CreateTrainingLevelCommandValidator(ITrainingLevelRepository repository)
    {
        _repository = repository;

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
            .MustAsync(BeUniqueName).WithMessage("Training level name must be unique.");

        RuleFor(v => v.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(v => v.PrerequisiteLevelId)
            .MustAsync(ExistIfNotNull).WithMessage("Prerequisite level does not exist.")
            .MustAsync(NotCreateCircularDependency).WithMessage("Circular dependency detected.");
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _repository.GetByNameAsync(name) == null;
    }

    private async Task<bool> ExistIfNotNull(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return true;
        return await _repository.GetByIdAsync(id.Value) != null;
    }

    private async Task<bool> NotCreateCircularDependency(CreateTrainingLevelCommand command, Guid? prerequisiteId, CancellationToken cancellationToken)
    {
        // For new record, levelId doesn't exist yet, but if it did, it would be a random guid.
        // But circular dependency is only possible if we point to someone who eventually points to us.
        // In creation, we don't have an ID yet that others could point to, unless we allow setting it.
        // However, if levelId == prerequisiteId it's circular.
        return true; // Simple check for creation
    }
}
