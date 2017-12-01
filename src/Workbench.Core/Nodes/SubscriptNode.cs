using System;
using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter.Ast;

namespace Workbench.Core.Nodes
{
    public class SubscriptNode : AstNode
    {
        public int Subscript { get; private set; }

#if false
        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
#endif

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Subscript = Convert.ToInt32(treeNode.Token.Value);
        }
    }
}