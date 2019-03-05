using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    internal sealed class ConstraintExpression
    {
        internal ConstraintExpressionNode Node { get; }

        internal ConstraintExpression(ConstraintExpressionNode expressionNode)
        {
            Node = expressionNode;
        }
    }
}