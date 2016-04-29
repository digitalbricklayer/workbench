using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class ExpanderScopeNode : ConstraintExpressionBaseNode
    {
        public LiteralNode Start { get; private set; }
        public LiteralNode End { get; private set; }

        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Start = (LiteralNode) AddChild("start", treeNode.ChildNodes[0]);
            // child node at subscript 1 is the .. keyword
            End = (LiteralNode) AddChild("end", treeNode.ChildNodes[2]);
        }
    }
}
