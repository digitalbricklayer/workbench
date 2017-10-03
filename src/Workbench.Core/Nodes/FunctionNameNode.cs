using System;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class FunctionNameNode : AstNode
    {
        /// <summary>
        /// Gets the function name.
        /// </summary>
        public string Name { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Name = Convert.ToString(treeNode.Token.Value);
        }
    }
}