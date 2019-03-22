using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// A node containing a literal.
    /// </summary>
    internal sealed class LiteralNode : Node
    {
        /// <summary>
        /// Initialize a variable node with a variable.
        /// </summary>
        /// <param name="literal">Literal content of the node.</param>
        internal LiteralNode(int literal)
        {
            Literal = literal;
        }

        /// <summary>
        /// Gets the literal inside the node.
        /// </summary>
        internal int Literal { get; }

        /// <summary>
        /// Gets whether the arc is solved.
        /// </summary>
        internal override bool IsSolved()
        {
            // A literal is always solved.
            return true;
        }
    }
}