using System;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class NumberLiteralNode : AstNode
    {
        /// <summary>
        /// Gets the number literal value.
        /// </summary>
        public int Value { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Value = Convert.ToInt32(treeNode.Token.Value);
        }
    }
}