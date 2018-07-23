using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public abstract class ConstraintItemViewModel : ItemViewModel
    {
        private string _expressionText;

        protected ConstraintItemViewModel(ConstraintModel theConstraint)
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