using System.Collections.Generic;
using Irony.Interpreter.Ast;
using Workbench.Core.Nodes;

namespace Workbench.Core
{
    /// <summary>
    /// Visitor to record information used when validating the constraint 
    /// expression from the abstract syntax tree.
    /// </summary>
    public class ConstraintExpressionValidatorVisitor : IAstVisitor
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

        public void BeginVisit(IVisitableNode node)
        {
            switch (node)
            {
                case AggregateVariableReferenceNode aggregateVariableReferenceNode:
                    this.aggregateVariableReferences.Add(aggregateVariableReferenceNode);
                    break;

                case SingletonVariableReferenceNode singletonVariableReferenceNode:
                    this.singletonVariableReferences.Add(singletonVariableReferenceNode);
                    break;
            }
        }

        public void EndVisit(IVisitableNode node)
        {
        }
    }
}
