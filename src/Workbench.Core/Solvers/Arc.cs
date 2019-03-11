namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Arc of a constraint network.
    /// </summary>
    internal sealed class Arc
    {
        /// <summary>
        /// Initialize an arc with a left and right sides and connecting constraint.
        /// </summary>
        /// <param name="left">Left variable.</param>
        /// <param name="right">Right variable.</param>
        /// <param name="constraint">Constraint connecting the two variables.</param>
        internal Arc(IntegerVariable left, IntegerVariable right, ConstraintExpression constraint)
        {
            Left = left;
            Right = right;
            Constraint = constraint;
        }

        /// <summary>
        /// Gets the variable on the left side of the arc.
        /// </summary>
        internal IntegerVariable Left { get; }

        /// <summary>
        /// Gets the variable on the right side of the arc.
        /// </summary>
        internal IntegerVariable Right { get; }

        /// <summary>
        /// Gets the connecting constraint.
        /// </summary>
        internal ConstraintExpression Constraint { get; }

        /// <summary>
        /// Gets whether the arc is solved.
        /// </summary>
        internal bool IsSolved => !Left.Domain.IsEmpty && !Right.Domain.IsEmpty;
    }
}