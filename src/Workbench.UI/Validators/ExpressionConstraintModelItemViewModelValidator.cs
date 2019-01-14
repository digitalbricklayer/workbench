using FluentValidation;
using Workbench.ViewModels;

namespace Workbench.Validators
{
    /// <summary>
    /// A strict validator for the singleton variable model item.
    /// </summary>
    public sealed class ExpressionConstraintModelItemViewModelValidator : AbstractValidator<ExpressionConstraintModelItemViewModel>
    {
        public ExpressionConstraintModelItemViewModelValidator()
        {
            RuleFor(item => item.DisplayName).NotNull().NotEmpty();
            RuleFor(item => item.ExpressionText).NotNull().NotEmpty().Must(ValidateExpression);
        }

        private bool ValidateExpression(ExpressionConstraintModelItemViewModel allDifferentConstraintModelItem, string rawDomainExpression)
        {
            var expressionConstraint = allDifferentConstraintModelItem.ExpressionConstraint;
            return expressionConstraint.Validate(expressionConstraint.GetModel());
        }
    }
}
