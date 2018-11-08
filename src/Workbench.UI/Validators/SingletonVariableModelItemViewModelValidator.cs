using FluentValidation;
using Workbench.ViewModels;

namespace Workbench.Validators
{
    /// <summary>
    /// A strict validator for the singleton variable model item.
    /// </summary>
    public sealed class SingletonVariableModelItemViewModelValidator : AbstractValidator<SingletonVariableModelItemViewModel>
    {
        public SingletonVariableModelItemViewModelValidator()
        {
            RuleFor(variable => variable.DisplayName).NotNull().NotEmpty();
            RuleFor(item => item.DomainExpressionText).NotNull().NotEmpty().Must(ValidateExpression);
        }

        private bool ValidateExpression(SingletonVariableModelItemViewModel singletonVariableModelItem, string rawDomainExpression)
        {
            var aggregateVariable = singletonVariableModelItem.SingletonVariable;
            return aggregateVariable.Validate(aggregateVariable.Parent);
        }
    }
}
