using FluentValidation;
using Workbench.ViewModels;

namespace Workbench.Validators
{
    /// <summary>
    /// A strict validator for the singleton variable model item.
    /// </summary>
    public sealed class AllDifferentConstraintModelItemViewModelValidator : AbstractValidator<AllDifferentConstraintModelItemViewModel>
    {
        public AllDifferentConstraintModelItemViewModelValidator()
        {
            RuleFor(item => item.DisplayName).NotNull().NotEmpty();
            RuleFor(item => item.ExpressionText).NotNull().NotEmpty().Must(ValidateExpression);
        }

        private bool ValidateExpression(AllDifferentConstraintModelItemViewModel allDifferentConstraintModelItem, string rawDomainExpression)
        {
            var allDifferentConstraint = allDifferentConstraintModelItem.AllDifferentConstraint;
            return allDifferentConstraint.Validate(allDifferentConstraint.Parent);
        }
    }
}
