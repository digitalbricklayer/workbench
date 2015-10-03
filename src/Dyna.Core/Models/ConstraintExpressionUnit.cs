using System;

namespace Dyna.Core.Models
{
    [Serializable]
    public class ConstraintExpressionUnit
    {
        public ConstraintExpressionUnit(VariableModel lhs, Expression rhs, OperatorType operatorType)
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
        public VariableModel Left { get; private set; }

        /// <summary>
        /// Gets the right hand side expression.
        /// </summary>
        public Expression Right { get; private set; }

        /// <summary>
        /// Gets the operator type of the expression.
        /// </summary>
        public OperatorType OperatorType { get; private set; }
    }
}
