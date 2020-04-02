using FluentValidation;
using Workbench.ViewModels;

namespace Workbench.Validators
{
    public sealed class BundleModelItemValidator : AbstractValidator<BundleModelItemViewModel>
    {
        public BundleModelItemValidator()
        {
            RuleFor(bundle => bundle.DisplayName).NotEmpty();
        }
    }
}