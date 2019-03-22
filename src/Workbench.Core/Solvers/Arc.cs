using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Arc of a constraint network.
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

            A = left;
            B = right;
            Connector = connector;
        }

        /// <summary>
        /// Gets the variable on the left side of the arc.
        /// </summary>
//        internal IntegerVariable Left => A.Variable;

        internal Node A { get; }

        /// <summary>
        /// Gets the variable on the right side of the arc.
        /// </summary>
//        internal IntegerVariable Right => B.Variable;

        internal Node B { get; }

        /// <summary>
        /// Gets the connecting constraint.
        /// </summary>
        internal ConstraintExpression Constraint => Connector.Constraint;

        internal NodeConnector Connector { get; }

        /// <summary>
        /// Gets whether the arc is solved.
        /// </summary>
        internal bool IsSolved => A.IsSolved() && B.IsSolved();
    }
}