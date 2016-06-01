using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class IfStatementNode : AstNode
    {
        public BinaryExpressionNode Expression { get; private set; }

        public CallStatementNode Statement { get; set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Expression = (BinaryExpressionNode) AddChild("Inner", treeNode.ChildNodes[0]);
            Statement = (CallStatementNode) AddChild("statement", treeNode.ChildNodes[1]);
        }
    }
}
