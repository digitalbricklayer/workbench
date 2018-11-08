using FluentValidation;
using Workbench.Core;
using Workbench.ViewModels;

namespace Workbench.Validators
{
    /// <summary>
    /// A strict validator for the shared domain model item.
    /// </summary>
    public sealed class SharedDomainModelItemViewModelValidator : AbstractValidator<SharedDomainModelItemViewModel>
    {
        public SharedDomainModelItemViewModelValidator()
        {
            RuleFor(item => item.DisplayName).NotNull().NotEmpty();
            RuleFor(item => item.ExpressionText).NotNull().NotEmpty().Must(ValidateExpression);
        }

        private bool ValidateExpression(SharedDomainModelItemViewModel sharedDomainModelItem, string rawDomainExpression)
        {
            var sharedDomain = sharedDomainModelItem.Domain;
            return sharedDomain.Validate(sharedDomain.Parent, new ModelValidationContext());
        }
    }
}
