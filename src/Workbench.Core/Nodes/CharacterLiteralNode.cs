using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using System;

namespace Workbench.Core.Nodes
{
    public class CharacterLiteralNode : AstNode
    {
        /// <summary>
        /// Gets the number literal value.
        /// </summary>
        public char Value { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Value = Convert.ToChar(treeNode.Token.Value);
        }
    }
}
