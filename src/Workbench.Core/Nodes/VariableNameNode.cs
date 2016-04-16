using System;
using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class VariableNameNode : ConstraintExpressionBaseNode
    {
        public string Name { get; private set; }

        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Name = Convert.ToString(treeNode.Token.Value);
        }
    }
}