using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A constraint restricts the values that can be bound to a variable.
    /// </summary>
    [Serializable]
    public class ConstraintModel : GraphicModel
    {
        private ConstraintExpressionModel expression;

        /// <summary>
        /// Initialize a constraint with a name and a constraint expression.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        /// <param name="location">Location of the graphic.</param>
        /// <param name="theExpression">Binary expression expressing the constraint.</param>
        public ConstraintModel(string constraintName, Point location, ConstraintExpressionModel theExpression)
            : base(constraintName, location)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(constraintName));
            Contract.Requires<ArgumentNullException>(theExpression != null);
            this.Expression = theExpression;
        }

        /// <summary>
        /// Initialize a constraint with a name and constraint expression.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        /// <param name="rawExpression">Raw constraint expression.</param>
        public ConstraintModel(string constraintName, string rawExpression)
            : base(constraintName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(constraintName));
            Contract.Requires<ArgumentNullException>(rawExpression != null);
            this.Expression = new ConstraintExpressionModel(rawExpression);
        }

        /// <summary>
        /// Initialize a constraint with a constraint expression.
        /// </summary>
        /// <param name="rawExpression">Raw constraint expression.</param>
        public ConstraintModel(string rawExpression)
            : this()
        {
            Contract.Requires<ArgumentNullException>(rawExpression != null);
            this.Expression = new ConstraintExpressionModel(rawExpression);
        }

        /// <summary>
        /// Initialize a constraint with an expression.
        /// </summary>
        /// <param name="theExpression">Binary expression expressing the constraint.</param>
        public ConstraintModel(ConstraintExpressionModel theExpression)
            : this()
        {
            Contract.Requires<ArgumentNullException>(theExpression != null);
            this.Expression = theExpression;
        }

        /// <summary>
        /// Initialize a constraint with a raw constraint expression.
        /// </summary>
        public ConstraintModel()
            : base("New constraint")
        {
            this.Expression = new ConstraintExpressionModel();
        }

        /// <summary>
        /// Gets or sets the constraint expression.
        /// </summary>
        public ConstraintExpressionModel Expression
        {
            get { return expression; }
            set
            {
                expression = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Parse the raw expression text.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        /// <param name="expressionText">Raw constraint expression.</param>
        public static ConstraintModel ParseExpression(string constraintName, string expressionText)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(constraintName));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(expressionText));

            return new ConstraintModel(constraintName, expressionText);
        }
    }
}
