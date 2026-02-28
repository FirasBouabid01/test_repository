using Domain.Interfaces;
using FluentValidation;

namespace Application.TrainingLevels.Commands.UpdateTrainingLevel;

public class UpdateTrainingLevelCommandValidator : AbstractValidator<UpdateTrainingLevelCommand>
{
    private readonly ITrainingLevelRepository _repository;

    public UpdateTrainingLevelCommandValidator(ITrainingLevelRepository repository)
    {
        _repository = repository;

        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
            .MustAsync(async (command, name, ct) => await BeUniqueName(command.Id, name, ct))
            .WithMessage("Training level name must be unique.");

        RuleFor(v => v.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(v => v.PrerequisiteLevelId)
            .MustAsync(ExistIfNotNull).WithMessage("Prerequisite level does not exist.")
            .MustAsync(async (command, prereqId, ct) => !await _repository.IsCircularDependencyAsync(command.Id, prereqId))
            .WithMessage("Circular dependency detected.");
    }

    private async Task<bool> BeUniqueName(Guid id, string name, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByNameAsync(name);
        return existing == null || existing.Id == id;
    }

    private async Task<bool> ExistIfNotNull(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return true;
        return await _repository.GetByIdAsync(id.Value) != null;
    }
}
