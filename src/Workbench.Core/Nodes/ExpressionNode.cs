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
                var singleton = InnerExpression as SingletonVariableReferenceNode;
                return singleton != null;
            }
        }

        public bool IsAggregateReference
        {
            get
            {
                var aggregate = InnerExpression as AggregateVariableReferenceNode;
                return aggregate != null;
            }
        }

        public bool IsVarable => IsSingletonReference || IsAggregateReference;

        public bool IsExpression => IsSingletonExpression || IsAggregateExpression;

        public bool IsSingletonExpression
        {
            get
            {
                var singletonExpression = InnerExpression as SingletonVariableReferenceExpressionNode;
                return singletonExpression != null;
            }
        }

        public bool IsAggregateExpression
        {
            get
            {
                var aggregateExpression = InnerExpression as AggregateVariableReferenceExpressionNode;
                return aggregateExpression != null;
            }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            InnerExpression = AddChild("Inner", treeNode.ChildNodes[0]);
        }

        public int GetLiteral()
        {
            Debug.Assert(!IsVarable);
            var literalNode = InnerExpression as IntegerLiteralNode;
            Debug.Assert(literalNode != null);
            return literalNode.Value;
        }
    }
}
