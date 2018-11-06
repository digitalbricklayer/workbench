using FluentValidation;
using Workbench.ViewModels;

namespace Workbench.Validators
{
    /// <summary>
    /// Validator for the singleton variable editor view model.
    /// </summary>
    public sealed class SingletonVariableEditorViewModelValidator : AbstractValidator<SingletonVariableEditorViewModel>
    {
        /// <summary>
        /// Initialize the singleton variable editor view model validator with default values.
        /// </summary>
        public SingletonVariableEditorViewModelValidator()
        {
            RuleFor(variable => variable.VariableName).NotEmpty().NotNull();
            RuleFor(variable => variable.DomainExpression).NotNull().NotEmpty();
        }
    }
}