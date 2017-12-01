using System.Diagnostics.Contracts;
using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter.Ast;

namespace Workbench.Core.Nodes
{
    public class SubscriptStatementNode : AstNode
    {
        public AstNode InnerExpression { get; private set; }

#if false
        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
#endif

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
            InnerExpression = AddChild("Inner", treeNode.ChildNodes[0]);
        }
    }
}
