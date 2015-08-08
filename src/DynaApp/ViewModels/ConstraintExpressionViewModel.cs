using System;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// A constraint expression view model.
    /// </summary>
    public sealed class ConstraintExpressionViewModel : AbstractViewModel
    {
        private string expression;

        /// <summary>
        /// Initialize a constraint expression with a raw expression.
        /// </summary>
        public ConstraintExpressionViewModel(string rawExpression)
        {
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
            this.Expression = rawExpression;
        }

        /// <summary>
        /// Initialize a constraint expression with default values.
        /// </summary>
        public ConstraintExpressionViewModel()
        {
            this.Expression = string.Empty;
        }

        /// <summary>
        /// Gets or sets the constraint expression.
        /// </summary>
        public string Expression
        {
            get { return this.expression; }
            set
            {
                if (this.expression == value) return;
                this.expression = value;
                OnPropertyChanged();
            }
        }
    }
}
