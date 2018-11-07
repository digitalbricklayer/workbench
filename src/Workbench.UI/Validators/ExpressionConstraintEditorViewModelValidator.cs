using FluentValidation;
using Workbench.ViewModels;

namespace Workbench.Validators
{
    /// <summary>
    /// Validator for the expression constraint editor view model.
    /// </summary>
    public sealed class ExpressionConstraintEditorViewModelValidator : AbstractValidator<ExpressionConstraintEditorViewModel>
    {
        /// <summary>
        /// Initialize the expression constraint editor view model validator with default values.
        /// </summary>
        public ExpressionConstraintEditorViewModelValidator()
        {
            RuleFor(variable => variable.ConstraintName).NotEmpty().NotNull();
            RuleFor(variable => variable.ConstraintExpression).NotNull().NotEmpty();
        }
    }
}