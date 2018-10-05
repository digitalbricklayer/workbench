using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class IfStatementNode : AstNode
    {
        public VisualizerBinaryExpressionNode Expression { get; private set; }

        public AstNode Statement { get; set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Expression = (VisualizerBinaryExpressionNode) AddChild("Inner", treeNode.ChildNodes[0]);
            Statement = AddChild("statement", treeNode.ChildNodes[1]);
        }
    }
}
