using FluentValidation;

namespace Workbench.ViewModels.Validators
{
    /// <summary>
    /// Validator for the shared domain editor view model.
    /// </summary>
    public class SharedDomainEditorViewModelValidator : AbstractValidator<SharedDomainEditorViewModel>
    {
        /// <summary>
        /// Initialize a new shared domain editor view model validator with default values.
        /// </summary>
        public SharedDomainEditorViewModelValidator()
        {
            RuleFor(domain => domain.DomainName).NotEmpty().NotNull();
            RuleFor(domain => domain.DomainExpression).NotEmpty().NotNull();
        }
    }
}
