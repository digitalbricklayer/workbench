using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class DomainExpressionNode : AstNode
    {
        public BandExpressionNode LeftExpression { get; set; }
        public BandExpressionNode RightExpression { get; set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            LeftExpression = (BandExpressionNode) AddChild("LHS", treeNode.ChildNodes[0]);
            RightExpression = (BandExpressionNode) AddChild("RHS", treeNode.ChildNodes[1]);
        }
    }
}