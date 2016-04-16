using System;
using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class LiteralNode : ConstraintExpressionBaseNode
    {
        public int Value { get; private set; }

        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Value = Convert.ToInt32(treeNode.Token.Value);
        }
    }
}
