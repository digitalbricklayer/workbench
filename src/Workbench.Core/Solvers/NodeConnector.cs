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
}
