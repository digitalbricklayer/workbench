using System;

namespace DynaApp.Entities
{
    class Constraint
    {
        /// <summary>
        /// Gets or sets the constraint expression.
        /// </summary>
        public BinaryExpression Expression { get; set; }

        /// <summary>
        /// Gets or sets the model the constraint is a part of.
        /// </summary>
        public Model Model { get; set; }

        public Constraint(BinaryExpression theExpression)
        {
            if (theExpression == null)
                throw new ArgumentNullException("theExpression");
            this.Expression = theExpression;
        }

        public Constraint(string rawExpression)
        {
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
            this.ParseExpression(rawExpression);
        }

        public void ParseExpression(string expressionText)
        {
            if (string.IsNullOrWhiteSpace(expressionText))
                throw new ArgumentException("expressionText");
            this.Expression = ConstraintGrammar.Parse(expressionText);
        }
    }
}
