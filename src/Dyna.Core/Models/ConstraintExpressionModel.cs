using System;

namespace Dyna.Core.Models
{
    /// <summary>
    /// Expression tree of the constraint.
    /// </summary>
    [Serializable]
    public class ConstraintExpressionModel
    {
        public ConstraintExpressionModel(VariableModel lhs, Expression rhs, OperatorType operatorType)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (rhs == null)
                throw new ArgumentNullException("rhs");

            this.Left = lhs;
            this.Right = rhs;
            this.OperatorType = operatorType;
        }

        public ConstraintExpressionModel(string rawExpression)
        {
            this.Text = rawExpression;
        }

        public ConstraintExpressionModel()
        {
            this.Text = string.Empty;
        }

        public string Text { get; set; }

        /// <summary>
        /// Gets the left hand side of the expression.
        /// </summary>
        public VariableModel Left { get; private set; }

        /// <summary>
        /// Gets the right hand side expression.
        /// </summary>
        public Expression Right { get; private set; }

        /// <summary>
        /// Gets the operator type of the expression.
        /// </summary>
        public OperatorType OperatorType { get; private set; }

        /// <summary>
        /// Returns a string that represents the constraint expression.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Left, OperatorType, Right);
        }
    }
}