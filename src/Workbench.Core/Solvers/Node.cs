using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// A node in an arc connected to the same node in a unary constraint or another node in a binary constraint.
    /// </summary>
    internal sealed class Node
    {
        /// <summary>
        /// Initialize a node with an expression node.
        /// </summary>
        /// <param name="expressionNode">One side of a constraint expression.</param>
        internal Node(ExpressionNode expressionNode)
        {
            Expression = expressionNode;
        }

        /// <summary>
        /// Gets the expression forming the contents of the node.
        /// </summary>
        internal ExpressionNode Expression { get; }

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
