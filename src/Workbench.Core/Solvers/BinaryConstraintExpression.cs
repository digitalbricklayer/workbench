using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    internal sealed class BinaryConstraintExpression
    {
        internal BinaryConstraintExpression(ConstraintExpressionNode expressionNode)
        {
            Node = expressionNode;
        }

        internal ConstraintExpressionNode Node { get; }
    }
}
