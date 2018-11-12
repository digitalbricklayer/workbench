using FluentValidation;
using Workbench.ViewModels;

namespace Workbench.Validators
{
    /// <summary>
    /// Validator for the all different constraint editor view model.
    /// </summary>
    public class AllDifferentConstraintEditorViewModelValidator : AbstractValidator<AllDifferentConstraintEditorViewModel>
    {
        /// <summary>
        /// Initialize a new all different constraint editor view model validator with default values.
        /// </summary>
        public AllDifferentConstraintEditorViewModelValidator()
        {
            RuleFor(constraint => constraint.ConstraintName).NotNull().NotEmpty();
            RuleFor(constraint => constraint.SelectedVariable).NotNull();
        }
    }
}
