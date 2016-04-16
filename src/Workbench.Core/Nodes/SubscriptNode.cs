using System;
using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class SubscriptNode : ConstraintExpressionBaseNode
    {
        public int Subscript { get; private set; }

        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Subscript = Convert.ToInt32(treeNode.Token.Value);
        }
    }
}