using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Arc connecting two variables inside a constraint network.
    /// </summary>
    internal sealed class Arc
    {
        /// <summary>
        /// Initialize an arc with a left and right nodes and connecting constraint.
        /// </summary>
        /// <param name="left">Left variable.</param>
        /// <param name="right">Right variable.</param>
        /// <param name="connector">Connector connecting the two nodes.</param>
        internal Arc(Node left, Node right, NodeConnector connector)
        {
            Contract.Requires<ArgumentNullException>(left != null);
            Contract.Requires<ArgumentNullException>(right != null);
            Contract.Requires<ArgumentNullException>(connector != null);

            Left = left;
            Right = right;
            Connector = connector;
        }

        /// <summary>
        /// Gets the left node.
        /// </summary>
        internal Node Left { get; }

        /// <summary>
        /// Gets the right node.
        /// </summary>
        internal Node Right { get; }

        /// <summary>
        /// Gets the connector.
        /// </summary>
        internal NodeConnector Connector { get; }

        /// <summary>
        /// Gets whether the arc is pre-computed.
        /// </summary>
        /// <remarks>
        /// An arc is pre-computed when it is a ternary or unary expression.
        /// </remarks>
        internal bool IsPreComputed => Connector is EncapsulatedVariableConnector;

        /// <summary>
        /// Is the arc consistent.
        /// </summary>
        /// <returns>True if the arc is consistent, False if the arc is not.</returns>
        internal bool IsArcConsistent()
        {
            return Left.IsNodeConsistent() && Right.IsNodeConsistent();
        }
    }
}
