using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class BucketVariableReferenceExpressionNode : ExpressionNode
    {
        public BucketVariableReferenceNode VariableReference { get; private set; }
        public VariableExpressionOperatorType Operator { get; private set; }
        public InfixStatementNode InfixStatement { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            VariableReference = (BucketVariableReferenceNode) AddChild("bucket variable reference", treeNode.ChildNodes[0]);
            Operator = VariableExpressionOperatorParser.ParseFrom(treeNode.ChildNodes[1].FindTokenAndGetText());
            InfixStatement = (InfixStatementNode) AddChild("Infix Statement", treeNode.ChildNodes[2]);
        }
    }
}