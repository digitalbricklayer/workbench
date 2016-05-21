using System;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class CallArgumentStringValueNode : AstNode
    {
        public string Value { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Value = Convert.ToString(treeNode.Token.Value);
        }
    }
}
