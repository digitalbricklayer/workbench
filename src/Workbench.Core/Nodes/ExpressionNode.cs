using System.Diagnostics;
using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter.Ast;

namespace Workbench.Core.Nodes
{
    public class ExpressionNode : AstNode
    {
        public AstNode InnerExpression { get; private set; }

        public bool IsLiteral => InnerExpression.IsConstant();

        public bool IsSingletonReference
        {
            get
            {
                return InnerExpression is SingletonVariableReferenceNode;
            }
        }

        public bool IsAggregateReference
        {
            get
            {
                return InnerExpression is AggregateVariableReferenceNode;
            }
        }

        public bool IsVariable => IsSingletonReference || IsAggregateReference;

        public bool IsExpression => IsSingletonExpression || IsAggregateExpression;

        public bool IsSingletonExpression
        {
            get
            {
                return InnerExpression is SingletonVariableReferenceExpressionNode;
            }
        }

        public bool IsAggregateExpression
        {
            get
            {
                return InnerExpression is AggregateVariableReferenceExpressionNode;
            }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            InnerExpression = AddChild("Inner", treeNode.ChildNodes[0]);
        }

        public int GetLiteral()
        {
            Debug.Assert(!IsVariable);
            var literalNode = InnerExpression as IntegerLiteralNode;
            Debug.Assert(literalNode != null);
            return literalNode.Value;
        }
    }
}
