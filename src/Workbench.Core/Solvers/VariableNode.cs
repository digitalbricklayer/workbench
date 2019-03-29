using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Solvers
{
#if false
    /// <summary>
    /// A node containing a variable.
    /// </summary>
    internal sealed class VariableNode : Node
    {
        /// <summary>
        /// Initialize a variable node with a variable.
        /// </summary>
        /// <param name="variable">Variable content of the node.</param>
        internal VariableNode(IntegerVariable variable)
        {
            Contract.Requires<ArgumentNullException>(variable != null);
            Variable = variable;
        }

        /// <summary>
        /// Gets the variable inside the node.
        /// </summary>
        internal IntegerVariable Variable { get; }

        /// <summary>
        /// Gets whether the arc is solved.
        /// </summary>
        internal override bool IsSolved()
        {
            return !Variable.Domain.IsEmpty;
        }

        /// <summary>
        /// Get the variable domain.
        /// </summary>
        /// <returns>Variable domain.</returns>
        internal override DomainRange GetRange()
        {
            return Variable.Domain;
        }
    }
#endif
}