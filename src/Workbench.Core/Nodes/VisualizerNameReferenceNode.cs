using System;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class VisualizerNameReferenceNode : AstNode
    {
        public string Name { get; set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Name = Convert.ToString(treeNode.Token.Value);
        }
    }
}
