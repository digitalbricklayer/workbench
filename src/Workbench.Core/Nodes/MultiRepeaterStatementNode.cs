using System.Linq;
using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class MultiRepeaterStatementNode : ConstraintExpressionBaseNode
    {
        public ScopeDeclarationListNode ScopeDeclarations { get; private set; }
        public CounterDeclarationListNode CounterDeclarations { get; private set; }
        public StatementNode Statement { get; private set; }

        /// <summary>
        /// Accept a visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
            CounterDeclarations.Accept(visitor);
            ScopeDeclarations.Accept(visitor);
        }
        
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            // The expander statement can be empty and still valid...
            if (!treeNode.ChildNodes.Any()) return;
            CounterDeclarations = (CounterDeclarationListNode) AddChild("counter declarations", treeNode.ChildNodes[0]);
            ScopeDeclarations = (ScopeDeclarationListNode)AddChild("scope declarations", treeNode.ChildNodes[2]);
            if (treeNode.ChildNodes.Count > 3)
            {
                Statement = (StatementNode) AddChild("statement", treeNode.ChildNodes[3]);
            }
        }
    }
}
