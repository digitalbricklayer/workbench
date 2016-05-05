using System.Collections.Generic;
using Workbench.Core.Nodes;

namespace Workbench.Core
{
    /// <summary>
    /// Visitor to record information used when validating the constraint 
    /// expression from the abstract syntax tree.
    /// </summary>
    public class ConstraintExpressionValidatorVisitor : IConstraintExpressionVisitor
    {
        private readonly List<SingletonVariableReferenceNode> singletonVariableReferences;
        private readonly List<AggregateVariableReferenceNode> aggregateVariableReferences;

        public ConstraintExpressionValidatorVisitor()
        {
            this.singletonVariableReferences = new List<SingletonVariableReferenceNode>();
            this.aggregateVariableReferences = new List<AggregateVariableReferenceNode>();
        }

        public IReadOnlyCollection<SingletonVariableReferenceNode> SingletonVariableReferences => this.singletonVariableReferences;

        public IReadOnlyCollection<AggregateVariableReferenceNode> AggregateVariableReferences => this.aggregateVariableReferences;

        public void Visit(AggregateVariableReferenceNode theNode)
        {
            this.aggregateVariableReferences.Add(theNode);
        }

        public void Visit(SingletonVariableReferenceNode theNode)
        {
            this.singletonVariableReferences.Add(theNode);
        }

        public void Visit(BinaryExpressionNode theNode)
        {
            // Nothing to do...
        }

        public void Visit(SubscriptNode subscriptNode)
        {
            // Nothing to do...
        }

        public void Visit(ExpressionNode theNode)
        {
            // Nothing to do...
        }

        public void Visit(ConstraintExpressionNode theNode)
        {
            // Nothing to do...
        }

        public void Visit(LiteralNode theNode)
        {
            // Nothing to do...
        }

        public void Visit(VariableNameNode theNode)
        {
            // Nothing to do...
        }

        public void Visit(SingletonVariableReferenceExpressionNode theNode)
        {
            // Nothing to do...
        }

        public void Visit(AggregateVariableReferenceExpressionNode theNode)
        {
            // Nothing to do...
        }

        public void Visit(SubscriptStatementNode theNode)
        {
            // Nothing to do...
        }

        public void Visit(CounterDeclarationNode theNode)
        {
            // Nothing to do...
        }

        public void Visit(ExpanderStatementNode theNode)
        {
            // Nothing to do...
        }

        public void Visit(ExpanderScopeNode theNode)
        {
            // Nothing to do...
        }

        public void Visit(CounterReferenceNode theNode)
        {
            // Nothing to do...
        }

        public void Visit(MultiRepeaterStatementNode theNode)
        {
            // Nothing to do...
        }
    }
}
