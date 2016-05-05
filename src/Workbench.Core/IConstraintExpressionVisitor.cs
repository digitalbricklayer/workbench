using Workbench.Core.Nodes;

namespace Workbench.Core
{
    public interface IConstraintExpressionVisitor
    {
        void Visit(AggregateVariableReferenceNode theNode);
        void Visit(SingletonVariableReferenceNode theNode);
        void Visit(BinaryExpressionNode theNode);
        void Visit(SubscriptNode subscriptNode);
        void Visit(ExpressionNode theNode);
        void Visit(ConstraintExpressionNode theNode);
        void Visit(LiteralNode theNode);
        void Visit(VariableNameNode theNode);
        void Visit(SingletonVariableReferenceExpressionNode theNode);
        void Visit(AggregateVariableReferenceExpressionNode theNode);
        void Visit(SubscriptStatementNode theNode);
        void Visit(CounterDeclarationNode theNode);
        void Visit(ExpanderStatementNode theNode);
        void Visit(ExpanderScopeNode theNode);
        void Visit(CounterReferenceNode theNode);
        void Visit(MultiRepeaterStatementNode theNode);
    }
}
