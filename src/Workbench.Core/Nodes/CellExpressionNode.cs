using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class CellExpressionNode : AstNode
    {
        public AstNode InnerExpression { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            InnerExpression = AddChild("cell expression", treeNode.ChildNodes[0]);
        }
    }
}