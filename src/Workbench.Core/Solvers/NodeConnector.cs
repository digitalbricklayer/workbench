using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// A connection between two nodes in the case of a binary expression or a single node in the case of a unary constraint.
    /// </summary>
    internal abstract class NodeConnector
    {
        /// <summary>
        /// Initialize a connector with content.
        /// </summary>
        /// <param name="left">Left node.</param>
        /// <param name="right">Right node.</param>
        /// <param name="content">Content of the connector.</param>
        internal NodeConnector(Node left, Node right, object content)
        {
            Contract.Requires<ArgumentNullException>(left != null);
            Contract.Requires<ArgumentNullException>(right != null);
            Contract.Requires<ArgumentNullException>(content != null);
            Left = left;
            Right = right;
            Content = content;
        }

        /// <summary>
        /// Gets the node on the left side of the connector.
        /// </summary>
        internal Node Left { get; }

        /// <summary>
        /// Gets the node on the right side of the connector.
        /// </summary>
        internal Node Right { get; }

        /// <summary>
        /// Gets the content of the connection.
        /// </summary>
        internal object Content { get; }

        /// <summary>
        /// Gets whether the connector connects the same node.
        /// </summary>
        internal bool IsSelfReferential => ReferenceEquals(Left, Right);
    }

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

    internal sealed class EncapsulatedVariableConnector : NodeConnector
    {
        /// <summary>
        /// Initialize a connector with a constraint.
        /// </summary>
        /// <param name="left">Left node.</param>
        /// <param name="right">Right node.</param>
        /// <param name="selector">Index into the values.</param>
        internal EncapsulatedVariableConnector(Node left, Node right, EncapsulatedSelector selector)
            : base(left, right, selector)
        {
            Selector = selector;
        }

        /// <summary>
        /// Gets the constraint.
        /// </summary>
        internal EncapsulatedSelector Selector { get; }
    }

    internal sealed class EncapsulatedSelector
    {
        internal EncapsulatedSelector(int index)
        {
            Index = index;
        }

        internal int Index { get; }
    }
}
