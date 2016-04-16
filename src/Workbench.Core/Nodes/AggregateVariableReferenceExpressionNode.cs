using System;
using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class AggregateVariableReferenceExpressionNode : ConstraintExpressionBaseNode
    {
        public AggregateVariableReferenceNode VariableReference { get; private set; }
        public VariableExpressionOperatorType Operator { get; private set; }
        public LiteralNode Literal { get; private set; }

        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            VariableReference = (AggregateVariableReferenceNode) AddChild("Variable Reference", treeNode.ChildNodes[0]);
            Operator = VariableExpressionOperatorParser.ParseFrom(treeNode.ChildNodes[1].FindTokenAndGetText());
            Literal = (LiteralNode)AddChild("Literal", treeNode.ChildNodes[2]);
        }
    }
}
