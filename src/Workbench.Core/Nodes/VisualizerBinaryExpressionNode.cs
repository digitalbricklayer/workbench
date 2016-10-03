using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class VisualizerBinaryExpressionNode : AstNode
    {
        public VisualizerExpressionNode LeftExpression { get; private set; }
        public VisualizerExpressionNode RightExpression { get; private set; }
        public OperatorType Operator { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            LeftExpression = (VisualizerExpressionNode) AddChild("Left", treeNode.ChildNodes[0]);
            Operator = OperatorTypeParser.ParseOperatorFrom(treeNode.ChildNodes[1].FindTokenAndGetText());
            RightExpression = (VisualizerExpressionNode) AddChild("Right", treeNode.ChildNodes[2]);
        }
    }
}
