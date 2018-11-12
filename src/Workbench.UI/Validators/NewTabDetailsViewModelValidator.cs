using FluentValidation;
using Workbench.ViewModels;

namespace Workbench.Validators
{
    public class NewTabDetailsViewModelValidator : AbstractValidator<NewTabDetailsViewModel>
    {
        public NewTabDetailsViewModelValidator()
        {
            RuleFor(detailsViewModel => detailsViewModel.TabName).NotNull().NotEmpty();
            RuleFor(detailsViewModel => detailsViewModel.TabDescription).NotNull().NotEmpty();
        }
    }
}