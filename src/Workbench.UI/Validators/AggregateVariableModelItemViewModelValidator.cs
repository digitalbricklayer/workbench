using FluentValidation;
using Workbench.ViewModels;

namespace Workbench.Validators
{
    /// <summary>
    /// A strict validator for the aggregator variable model item.
    /// </summary>
    public sealed class AggregateVariableModelItemViewModelValidator : AbstractValidator<AggregateVariableModelItemViewModel>
    {
        public AggregateVariableModelItemViewModelValidator()
        {
            RuleFor(variable => variable.DisplayName).NotNull().NotEmpty();
            RuleFor(variable => variable.VariableCount).NotEmpty().GreaterThan(0);
            RuleFor(item => item.DomainExpressionText).NotNull().NotEmpty().Must(ValidateExpression);
        }

        private bool ValidateExpression(AggregateVariableModelItemViewModel aggregateVariableModelItem, string rawDomainExpression)
        {
            var aggregateVariable = aggregateVariableModelItem.AggregateVariable;
            return aggregateVariable.Validate(aggregateVariable.GetModel());
        }
    }
}
