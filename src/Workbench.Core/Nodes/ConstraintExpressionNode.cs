using System.Linq;
using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class ConstraintExpressionNode : ConstraintExpressionBaseNode
    {
        public BinaryExpressionNode InnerExpression { get; private set; }

        /// <summary>
        /// Gets whether the expression is empty.
        /// </summary>
        public bool IsEmpty => InnerExpression == null;

        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
            InnerExpression.Accept(visitor);
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            // Make sure the expression isn't empty
            if (treeNode.ChildNodes.Any())
                InnerExpression = (BinaryExpressionNode) AddChild("Inner", treeNode.ChildNodes[0]);
        }
    }
}
