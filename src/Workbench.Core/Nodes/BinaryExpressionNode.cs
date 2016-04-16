using System;
using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class BinaryExpressionNode : ConstraintExpressionBaseNode
    {
        public ExpressionNode LeftExpression { get; private set; }
        public ExpressionNode RightExpression { get; private set; }
        public OperatorType Operator { get; private set; }

        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
            LeftExpression.Accept(visitor);
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            LeftExpression = (ExpressionNode) AddChild("Left", treeNode.ChildNodes[0]);
            Operator = ParseOperatorFrom(treeNode.ChildNodes[1].FindTokenAndGetText());
            RightExpression = (ExpressionNode) AddChild("Right", treeNode.ChildNodes[2]);
        }

        private static OperatorType ParseOperatorFrom(string operratorToken)
        {
            switch (operratorToken)
            {
                case "<":
                    return OperatorType.Less;

                case "<=":
                    return OperatorType.LessThanOrEqual;

                case ">":
                    return OperatorType.Greater;

                case ">=":
                    return OperatorType.GreaterThanOrEqual;

                case "=":
                    return OperatorType.Equals;

                case "<>":
                    return OperatorType.NotEqual;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
