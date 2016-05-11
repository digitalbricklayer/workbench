using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class ExpanderScopeNode : ConstraintExpressionBaseNode
    {
        public ScopeLimitSatementNode Start { get; private set; }
        public ScopeLimitSatementNode End { get; private set; }
        public ExpanderCountNode Count { get; private set; }
        public bool IsCount => Count != null;
        public bool IsScope => Start != null && End != null;

        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            if (treeNode.ChildNodes.Count > 1)
            {
                Start = (ScopeLimitSatementNode) AddChild("start", treeNode.ChildNodes[0]);
                // child node at subscript 1 is the .. keyword
                End = (ScopeLimitSatementNode) AddChild("end", treeNode.ChildNodes[2]);
            }
            else
            {
                Count = (ExpanderCountNode) AddChild("count", treeNode.ChildNodes[0]);
            }
        }
    }
}
