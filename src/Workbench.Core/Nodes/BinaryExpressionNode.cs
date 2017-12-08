using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class BinaryExpressionNode : AstNode
    {
        public ExpressionNode LeftExpression { get; private set; }
        public ExpressionNode RightExpression { get; private set; }
        public OperatorType Operator { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            LeftExpression = (ExpressionNode) AddChild("Left", treeNode.ChildNodes[0]);
            Operator = OperatorTypeParser.ParseOperatorFrom(treeNode.ChildNodes[1].FindTokenAndGetText());
            RightExpression = (ExpressionNode) AddChild("Right", treeNode.ChildNodes[2]);
        }
    }
}
