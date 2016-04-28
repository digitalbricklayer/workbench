using System.Linq;
using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class ConstraintExpressionNode : ConstraintExpressionBaseNode
    {
        public BinaryExpressionNode InnerExpression { get; private set; }

        /// <summary>
        /// Gets the expander statement node.
        /// </summary>
        /// <remarks>
        /// The expander is an optional part of the constraint expression so 
        /// may be Null.
        /// </remarks>
        public ExpanderStatementNode Expander { get; private set; }

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
            // A constraint expression can be empty and still be perfectly valid...
            if (!treeNode.ChildNodes.Any()) return;
            InnerExpression = (BinaryExpressionNode) AddChild("Inner", treeNode.ChildNodes[0]);
            // The expander is optional...
            if (treeNode.ChildNodes.Count < 2) return;
            var expanderChildNodes = treeNode.ChildNodes[1].ChildNodes;
            if (expanderChildNodes.Any())
                Expander = (ExpanderStatementNode) AddChild("Expander", treeNode.ChildNodes[1]);
        }
    }
}
