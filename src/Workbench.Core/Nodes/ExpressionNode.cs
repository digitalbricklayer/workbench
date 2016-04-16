using System.Diagnostics;
using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class ExpressionNode : ConstraintExpressionBaseNode
    {
        public ConstraintExpressionBaseNode InnerExpression { get; private set; }

        public bool IsLiteral
        {
            get { return InnerExpression.IsConstant(); }
        }

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

        public bool IsVarable
        {
            get { return IsSingletonReference || IsAggregateReference; }
        }

        public bool IsExpression
        {
            get { return IsSingletonExpression || IsAggregateExpression; }
        }

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

        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
            InnerExpression.Accept(visitor);
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            InnerExpression = (ConstraintExpressionBaseNode) AddChild("Inner", treeNode.ChildNodes[0]);
        }

        public int GetLiteral()
        {
            Debug.Assert(!IsVarable);
            var literalNode = InnerExpression as LiteralNode;
            Debug.Assert(literalNode != null);
            return literalNode.Value;
        }
    }
}
