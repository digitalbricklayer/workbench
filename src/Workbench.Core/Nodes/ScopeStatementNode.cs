using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class ScopeStatementNode : AstNode
    {
        public ScopeLimitSatementNode Start { get; private set; }
        public ScopeLimitSatementNode End { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Start = (ScopeLimitSatementNode) AddChild("start", treeNode.ChildNodes[0]);
            End = (ScopeLimitSatementNode) AddChild("end", treeNode.ChildNodes[1]);
        }
    }
}