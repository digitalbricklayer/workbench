using System;

namespace Dyna.Core.Models
{
    /// <summary>
    /// A constraint restricts a variable's possible values.
    /// </summary>
    [Serializable]
    public class ConstraintModel : GraphicModel
    {
        /// <summary>
        /// Initialize a constraint with an expression.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        /// <param name="rawExpression">Raw constraint expression.</param>
        public ConstraintModel(string constraintName, string rawExpression)
            : base(constraintName)
        {
            this.Expression = new ConstraintExpressionModel(rawExpression);
        }

        /// <summary>
        /// Initialize a constraint with an expression.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        /// <param name="theExpression">Binary expression expressing the constraint.</param>
        public ConstraintModel(string constraintName, ConstraintExpressionModel theExpression)
            : base(constraintName)
        {
            if (theExpression == null)
                throw new ArgumentNullException("theExpression");
            this.Expression = theExpression;
        }

        /// <summary>
        /// Initialize a constraint with an expression.
        /// </summary>
        /// <param name="rawExpression">Raw constraint expression.</param>
        public ConstraintModel(string rawExpression)
            : this()
        {
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
            this.Expression = ConstraintGrammar.Parse(rawExpression);
        }

        /// <summary>
        /// Initialize a constraint with an expression.
        /// </summary>
        /// <param name="theExpression">Binary expression expressing the constraint.</param>
        public ConstraintModel(ConstraintExpressionModel theExpression)
            : this()
        {
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
        public ConstraintExpressionModel Expression { get; set; }

        /// <summary>
        /// Parse the raw expression text.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        /// <param name="expressionText">Raw constraint expression.</param>
        public static ConstraintModel ParseExpression(string constraintName, string expressionText)
        {
            if (string.IsNullOrWhiteSpace(constraintName))
                throw new ArgumentException("constraintName");
            if (string.IsNullOrWhiteSpace(expressionText))
                throw new ArgumentException("expressionText");
            return new ConstraintModel(constraintName, ConstraintGrammar.Parse(expressionText));
        }
    }
}
