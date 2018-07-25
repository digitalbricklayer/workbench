using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public abstract class ConstraintModelItemViewModel : ModelItemViewModel
    {
        private string _expressionText;

        protected ConstraintModelItemViewModel(ConstraintModel theConstraint)
            : base(theConstraint)
        {
            Constraint = theConstraint;
            DisplayName = theConstraint.Name;
        }

        /// <summary>
        /// Get or set the constraint expression text.
        /// </summary>
        public string ExpressionText
        {
            get => _expressionText;
            set
            {
                _expressionText = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the constraint model.
        /// </summary>
        public ConstraintModel Constraint { get; }
    }
}