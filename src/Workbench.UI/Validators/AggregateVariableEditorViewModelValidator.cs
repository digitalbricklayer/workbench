using FluentValidation;
using Workbench.ViewModels;

namespace Workbench.Validators
{
    /// <summary>
    /// Validator for the aggregate variable editor dialog.
    /// </summary>
    public class AggregateVariableEditorViewModelValidator : AbstractValidator<AggregateVariableEditorViewModel>
    {
        /// <summary>
        /// Initialize the aggregate variable editor validator with default values.
        /// </summary>
        public AggregateVariableEditorViewModelValidator()
        {
            RuleFor(variable => variable.VariableName).NotEmpty().NotNull();
            RuleFor(variable => variable.Size).NotEmpty().GreaterThan(0);
            RuleFor(variable => variable.DomainExpression).NotNull().NotEmpty();
        }
    }
}
