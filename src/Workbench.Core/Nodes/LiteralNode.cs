using System;
using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter.Ast;

namespace Workbench.Core.Nodes
{
    public class IntegerLiteralNode : AstNode
    {
        public int Value { get; private set; }

#if false
        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
#endif

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Value = Convert.ToInt32(treeNode.Token.Value);
        }
    }
}
