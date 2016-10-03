using System.Diagnostics.Contracts;
using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class SubscriptStatementNode : ConstraintExpressionBaseNode
    {
        public ConstraintExpressionBaseNode InnerExpression { get; private set; }

        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public bool IsSubscript
        {
            get
            {
                var literal = InnerExpression as SubscriptNode;
                return literal != null;
            }
        }

        public bool IsCounterReference
        {
            get
            {
                var counterReference = InnerExpression as CounterReferenceNode;
                return counterReference != null;
            }
        }

        public int Subscript
        {
            get
            {
                Contract.Assume(IsSubscript);
                var subscriptNode = (SubscriptNode) InnerExpression;
                return subscriptNode.Subscript;
            }
        }

        public CounterReferenceNode CounterReference => InnerExpression as CounterReferenceNode;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            InnerExpression = (ConstraintExpressionBaseNode) AddChild("Inner", treeNode.ChildNodes[0]);
        }
    }
}
