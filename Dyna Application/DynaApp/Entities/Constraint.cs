using System;

namespace DynaApp.Entities
{
    /// <summary>
    /// A constraint restricting a variable's possible bound values.
    /// </summary>
    public class Constraint
    {
        /// <summary>
        /// Initialize a constraint with an expression.
        /// </summary>
        /// <param name="theExpression">Binary expression expressing the constraint.</param>
        public Constraint(BinaryExpression theExpression)
        {
            if (theExpression == null)
                throw new ArgumentNullException("theExpression");
            this.Expression = theExpression;
        }

        /// <summary>
        /// Initialize a constraint with a raw constraint expression.
        /// </summary>
        /// <param name="rawExpression">Raw text expression expressing the constraint.</param>
        public Constraint(string rawExpression)
        {
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
            this.Expression = ConstraintGrammar.Parse(rawExpression);
        }

        /// <summary>
        /// Gets or sets the constraint name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the constraint expression.
        /// </summary>
        public BinaryExpression Expression { get; set; }

        /// <summary>
        /// Gets or sets the model the constraint is a part of.
        /// </summary>
        public Model Model { get; set; }

        /// <summary>
        /// Parse the raw expression text.
        /// </summary>
        /// <param name="expressionText">Raw constraint expression.</param>
        public static Constraint ParseExpression(string expressionText)
        {
            if (string.IsNullOrWhiteSpace(expressionText))
                throw new ArgumentException("expressionText");
            return new Constraint(ConstraintGrammar.Parse(expressionText));
        }
    }
}
