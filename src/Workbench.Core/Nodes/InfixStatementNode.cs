using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class InfixStatementNode : ConstraintExpressionBaseNode
    {
        public ConstraintExpressionBaseNode InnerExpression { get; private set; }

        public bool IsLiteral
        {
            get
            {
                var literal = InnerExpression as LiteralNode;
                return literal != null;
            }
        }

        public bool IsCounterReference
        {
            get
            {
                var counterReference = InnerExpression as CounterReferenceNode;
                return counterReference != null;
            }
        }

        public LiteralNode Literal
        {
            get
            {
                var literalNode = (LiteralNode) InnerExpression;
                return literalNode;
            }
        }

        public CounterReferenceNode CounterReference
        {
            get
            {
                var counterReference = (CounterReferenceNode) InnerExpression;
                return counterReference;
            }
        }

        /// <summary>
        /// Accept a visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            InnerExpression = (ConstraintExpressionBaseNode)AddChild("Inner", treeNode.ChildNodes[0]);
        }
    }
}
