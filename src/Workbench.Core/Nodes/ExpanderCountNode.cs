using System.Diagnostics.Contracts;
using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class ExpanderCountNode : ConstraintExpressionBaseNode
    {
        public ConstraintExpressionBaseNode InnerExpression { get; private set; }

        public LiteralNode Literal
        {
            get
            {
                Contract.Assume(IsLiteral);
                return (LiteralNode)InnerExpression;
            }
        }

        public CounterReferenceNode CounterReference
        {
            get
            {
                Contract.Assume(IsCounterReference);
                return (CounterReferenceNode)InnerExpression;
            }
        }

        public bool IsLiteral
        {
            get
            {
                var literal = InnerExpression as LiteralNode;
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

        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            throw new System.NotImplementedException();
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            // Can be either a literal or a counter reference...
            InnerExpression = (ConstraintExpressionBaseNode)AddChild("Inner", treeNode.ChildNodes[0]);
        }
    }
}
