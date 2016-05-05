using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class ExpanderScopeNode : ConstraintExpressionBaseNode
    {
        public ScopeLimitSatementNode Start { get; private set; }
        public ScopeLimitSatementNode End { get; private set; }

        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Start = (ScopeLimitSatementNode) AddChild("start", treeNode.ChildNodes[0]);
            // child node at subscript 1 is the .. keyword
            End = (ScopeLimitSatementNode) AddChild("end", treeNode.ChildNodes[2]);
        }
    }
}
