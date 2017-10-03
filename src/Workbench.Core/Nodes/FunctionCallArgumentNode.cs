using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using Workbench.Core.Grammars;

namespace Workbench.Core.Nodes
{
    public class FunctionCallArgumentNode : AstNode
    {
        /// <summary>
        /// Gets the argument string literal value.
        /// </summary>
        public FunctionCallArgumentStringLiteralNode Value { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Value = (FunctionCallArgumentStringLiteralNode) AddChild("argument value", treeNode.ChildNodes[0]);
        }
    }
}