using System.Diagnostics.Contracts;
using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class AggregateVariableReferenceNode : ConstraintExpressionBaseNode
    {
        public string VariableName { get; private set; }

#if true
        /// <summary>
        /// This is obsolete but retain to keep the compiler happy...
        /// </summary>
        public int Subscript
        {
            get
            {
                Contract.Assume(SubscriptStatement.IsSubscript);
                return SubscriptStatement.Subscript;
            }
        }
#endif

        public SubscriptStatementNode SubscriptStatement { get; private set; }

        /// <summary>
        /// Accept a visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
            SubscriptStatement.Accept(visitor);
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            VariableName = treeNode.ChildNodes[0].FindTokenAndGetText();
            // Child nodes 1 and 3 contain the opening and closing brackets
            SubscriptStatement = (SubscriptStatementNode) AddChild("Subscript Statement", treeNode.ChildNodes[2]);
        }
    }
}
