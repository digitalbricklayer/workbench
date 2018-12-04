using FluentValidation;
using Workbench.ViewModels;

namespace Workbench.Validators
{
    public sealed class VisualizerExpressionValidator : AbstractValidator<VisualizerExpressionEditorViewModel>
    {
        public VisualizerExpressionValidator()
        {
            RuleFor(expression => expression.Expression).NotNull().NotEmpty();
        }
    }
}