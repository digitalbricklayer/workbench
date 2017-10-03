using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class BandExpressionNode : AstNode
    {
        public AstNode Inner { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Inner = AddChild("inner", treeNode.ChildNodes[0]);
        }
    }
}