using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class CounterDeclarationListNode : ConstraintExpressionBaseNode
    {
        private readonly IList<CounterDeclarationNode> counterDeclarations;

        public CounterDeclarationListNode()
        {
            this.counterDeclarations = new List<CounterDeclarationNode>();
        }

        public IReadOnlyCollection<CounterDeclarationNode> CounterDeclarations
        {
            get
            {
                return new ReadOnlyCollection<CounterDeclarationNode>(this.counterDeclarations);
            }
        }

        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            foreach (var node in treeNode.ChildNodes)
            {
                var newCounterDeclaration = (CounterDeclarationNode) AddChild("counter declaration", node);
                this.counterDeclarations.Add(newCounterDeclaration);
            }
        }
    }
}
