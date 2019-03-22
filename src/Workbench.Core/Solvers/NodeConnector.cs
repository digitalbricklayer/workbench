using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Solvers
{
    internal sealed class NodeConnector
    {
        internal ConstraintExpression Constraint { get; }

        internal NodeConnector(ConstraintExpression constraint)
        {
            Contract.Requires<ArgumentNullException>(constraint != null);
            Constraint = constraint;
        }
    }
}