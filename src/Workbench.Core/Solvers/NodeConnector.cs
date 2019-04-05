using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// A connection between two nodes in the case of a binary expression or a single node in the case of a unary constraint.
    /// </summary>
    internal sealed class NodeConnector
    {
        /// <summary>
        /// Gets the node on the left side of the connector.
        /// </summary>
        internal Node Left { get; }

        /// <summary>
        /// Gets the node on the right side of the connector.
        /// </summary>
        internal Node Right { get; }

        /// <summary>
        /// Gets the constraint.
        /// </summary>
        internal ConstraintExpression Constraint { get; }

        /// <summary>
        /// Initialize a connector with a constraint.
        /// </summary>
        /// <param name="left">Left node.</param>
        /// <param name="right">Right node.</param>
        /// <param name="constraint">Constraint expression.</param>
        internal NodeConnector(Node left, Node right, ConstraintExpression constraint)
        {
            Contract.Requires<ArgumentNullException>(left != null);
            Contract.Requires<ArgumentNullException>(right != null);
            Contract.Requires<ArgumentNullException>(constraint != null);
            Left = left;
            Right = right;
            Constraint = constraint;
        }

        /// <summary>
        /// Gets whether the connector connects the same node.
        /// </summary>
        internal bool IsSelfReferential => ReferenceEquals(Left, Right);
    }
}
