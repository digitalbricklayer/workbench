using System;
using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter.Ast;

namespace Workbench.Core.Nodes
{
    public class CounterReferenceNode : AstNode
    {
        public string CounterName { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            CounterName = Convert.ToString(treeNode.Token.Value);
        }
    }
}
