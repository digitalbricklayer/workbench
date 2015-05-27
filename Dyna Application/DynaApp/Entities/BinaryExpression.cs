using System;

namespace DynaApp.Entities
{
    /// <summary>
    /// Expression tree of the constraint.
    /// </summary>
    class BinaryExpression
    {
        public Variable Left { get; set; }
        public Expression Right { get; set; }
        public OperatorType OperatorType { get; set; }

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
    }
}
