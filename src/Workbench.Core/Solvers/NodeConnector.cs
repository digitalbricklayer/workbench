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
        /// Gets the constraint.
        /// </summary>
        internal ConstraintExpression Constraint { get; }

        /// <summary>
        /// Initialize a connector with a constraint.
        /// </summary>
        /// <param name="constraint">Constraint expression.</param>
        internal NodeConnector(ConstraintExpression constraint)
        {
            Contract.Requires<ArgumentNullException>(constraint != null);
            Constraint = constraint;
        }
    }
}
