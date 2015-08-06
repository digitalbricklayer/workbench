using System;

namespace DynaApp.Entities
{
    /// <summary>
    /// Expression tree of the constraint.
    /// </summary>
    public class BinaryExpression
    {
        public BinaryExpression(Variable lhs, Expression rhs, OperatorType operatorType)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (rhs == null)
                throw new ArgumentNullException("rhs");

            this.Left = lhs;
            this.Right = rhs;
            this.OperatorType = operatorType;
        }

        /// <summary>
        /// Gets the left hand side of the expression.
        /// </summary>
        public Variable Left { get; private set; }

        /// <summary>
        /// Gets the right hand side expression.
        /// </summary>
        public Expression Right { get; private set; }

        /// <summary>
        /// Gets the operator type of the expression.
        /// </summary>
        public OperatorType OperatorType { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Left, OperatorType, Right);
        }
    }
}
