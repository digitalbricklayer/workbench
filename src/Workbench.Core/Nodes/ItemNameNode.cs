using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using System;

namespace Workbench.Core.Nodes
{
    public class ItemNameNode : AstNode
    {
        public string Value { get; private set; }

        public ItemNameNode()
        {
            Value = string.Empty;
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Value = Convert.ToString(treeNode.Token.Value);
        }
    }
}
