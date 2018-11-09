using FluentValidation;
using Workbench.ViewModels;

namespace Workbench.Validators
{
    /// <summary>
    /// Validate the tab name editor view model.
    /// </summary>
    public class TabNameValidator : AbstractValidator<TabNameEditorViewModel>
    {
        public TabNameValidator()
        {
            RuleFor(name => name.TabName).NotNull().NotEmpty();
        }
    }
}
