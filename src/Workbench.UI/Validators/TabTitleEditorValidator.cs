using FluentValidation;
using Workbench.ViewModels;

namespace Workbench.Validators
{
    /// <summary>
    /// Validator for the tab title editor view model.
    /// </summary>
    public class TabTitleEditorValidator : AbstractValidator<TabTitleEditorViewModel>
    {
        public TabTitleEditorValidator()
        {
            RuleFor(title => title.TabTitle).NotNull().NotEmpty();
        }
    }
}