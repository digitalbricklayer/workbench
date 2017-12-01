using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class ExpanderScopeNode : AstNode
    {
        public ScopeStatementNode Scope { get; private set; }
        public ExpanderCountNode Count { get; private set; }
        public bool IsCount => Count != null;
        public bool IsScope => Scope != null;

#if false
        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
#endif

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            if (treeNode.ChildNodes[0].Term.Name == "scope")
            {
                Scope = (ScopeStatementNode) AddChild("scope", treeNode.ChildNodes[0]);
            }
            else
            {
                Count = (ExpanderCountNode) AddChild("count", treeNode.ChildNodes[0]);
            }
        }
    }
}
