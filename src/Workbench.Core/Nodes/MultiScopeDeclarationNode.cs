using System.Collections.Generic;
using System.Collections.ObjectModel;
using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter.Ast;

namespace Workbench.Core.Nodes
{
    public class ScopeDeclarationListNode : AstNode
    {
        private readonly IList<ExpanderScopeNode> scopeDeclarations;

        public ScopeDeclarationListNode()
        {
            this.scopeDeclarations = new List<ExpanderScopeNode>();
        }

        public IReadOnlyCollection<ExpanderScopeNode> ScopeDeclarations
        {
            get
            {
                return new ReadOnlyCollection<ExpanderScopeNode>(this.scopeDeclarations);
            }
        }

#if false
        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
#endif

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            foreach (var node in treeNode.ChildNodes)
            {
                var newScopeDeclaration = (ExpanderScopeNode) AddChild("scope declaration", node);
                this.scopeDeclarations.Add(newScopeDeclaration);
            }
        }
    }
}