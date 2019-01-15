using FluentValidation;
using Workbench.ViewModels;

namespace Workbench.Validators
{
    public sealed class BundleNameEditorViewModelValidator : AbstractValidator<BundleNameEditorViewModel>
    {
        public BundleNameEditorViewModelValidator()
        {
            RuleFor(bundle => bundle.BundleName).NotEmpty();
        }
    }
}