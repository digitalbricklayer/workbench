namespace Workbench.Core.Solvers
{
    internal sealed class ConstraintExpressionConnector : NodeConnector
    {
        /// <summary>
        /// Initialize a connector with a constraint.
        /// </summary>
        /// <param name="left">Left node.</param>
        /// <param name="right">Right node.</param>
        /// <param name="constraint">Constraint connecting the two nodes.</param>
        internal ConstraintExpressionConnector(Node left, Node right, BinaryConstraintExpression constraint)
            : base(left, right, constraint)
        {
            Constraint = constraint;
        }

        /// <summary>
        /// Gets the constraint.
        /// </summary>
        internal BinaryConstraintExpression Constraint { get; }
    }
}
