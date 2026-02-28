using FluentValidation;

namespace Application.TrainingLevels.Commands.ReorderLevels;

public class ReorderLevelsCommandValidator : AbstractValidator<ReorderLevelsCommand>
{
    public ReorderLevelsCommandValidator()
    {
        RuleFor(v => v.Levels)
            .NotEmpty().WithMessage("Levels list cannot be empty.");

        RuleForEach(v => v.Levels).ChildRules(level =>
        {
            level.RuleFor(l => l.Id).NotEmpty().WithMessage("Id is required.");
            level.RuleFor(l => l.SequenceOrder).GreaterThanOrEqualTo(0).WithMessage("Sequence order must be non-negative.");
        });
    }
}
