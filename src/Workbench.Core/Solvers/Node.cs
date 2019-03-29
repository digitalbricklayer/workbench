using System;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    internal sealed class Node
    {
        /// <summary>
        /// Initialize a node with an expression node.
        /// </summary>
        /// <param name="expressionNode"></param>
        internal Node(ExpressionNode expressionNode)
        {
            Expression = expressionNode;
        }

        /// <summary>
        /// Gets the expression forming the contents of the node.
        /// </summary>
        internal ExpressionNode Expression { get; }

        /// <summary>
        /// Gets whether the node has been solved.
        /// </summary>
        /// <returns>True if the node is solved, false if the node is not solved.</returns>
        internal bool IsSolved()
        {
            return !GetRange().IsEmpty;
        }

        /// <summary>
        /// Get the domain for the node.
        /// </summary>
        /// <returns>Domain range.</returns>
        internal DomainRange GetRange()
        {
            return new DomainRange(Array.Empty<int>());
        }

        /// <summary>
        /// Get whether the expression is revisable.
        /// </summary>
        /// <returns>True if the expression is revisable, False if it is not revisable.</returns>
        internal bool IsRevisable()
        {
            return !Expression.IsLiteral;
        }
    }
}
