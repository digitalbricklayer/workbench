using System.Collections.Generic;
using Irony.Interpreter.Ast;
using Workbench.Core.Nodes;
using System.Diagnostics.Contracts;
using System;

namespace Workbench.Core
{
    /// <summary>
    /// Visitor to record information used when validating the constraint 
    /// expression from the abstract syntax tree.
    /// </summary>
    public class ConstraintVariableReferenceCaptureVisitor : IAstVisitor
    {
        private readonly List<SingletonVariableReferenceNode> singletonVariableReferences;
        private readonly List<AggregateVariableReferenceNode> aggregateVariableReferences;

        public ConstraintVariableReferenceCaptureVisitor()
        {
            this.singletonVariableReferences = new List<SingletonVariableReferenceNode>();
            this.aggregateVariableReferences = new List<AggregateVariableReferenceNode>();
        }

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

        public VariableReferences GetReferences()
        {
            return new VariableReferences(this.singletonVariableReferences, this.aggregateVariableReferences);
        }

        public class VariableReferences
        {
            private readonly List<SingletonVariableReferenceNode> singletonVariableReferences;
            private readonly List<AggregateVariableReferenceNode> aggregateVariableReferences;

            public VariableReferences(IEnumerable<SingletonVariableReferenceNode> theSingletonReferences, IEnumerable<AggregateVariableReferenceNode> theAggregateReferences)
            {
                Contract.Requires<ArgumentNullException>(theSingletonReferences != null);
                Contract.Requires<ArgumentNullException>(theAggregateReferences != null);

                this.singletonVariableReferences = new List<SingletonVariableReferenceNode>(theSingletonReferences);
                this.aggregateVariableReferences = new List<AggregateVariableReferenceNode>(theAggregateReferences);
            }

            /// <summary>
            /// Gets all singleton variable references.
            /// </summary>
            public IReadOnlyCollection<SingletonVariableReferenceNode> SingletonVariableReferences => this.singletonVariableReferences;

            /// <summary>
            /// Gets all aggregate variable references.
            /// </summary>
            public IReadOnlyCollection<AggregateVariableReferenceNode> AggregateVariableReferences => this.aggregateVariableReferences;
        }
    }
}
