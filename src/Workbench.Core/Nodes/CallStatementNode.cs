using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class CallStatementNode : AstNode
    {
        public VisualizerNameReferenceNode VizualizerName { get; private set; }
        public CallArgumentListNode Arguments { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            VizualizerName = (VisualizerNameReferenceNode) AddChild("visualizer name", treeNode.ChildNodes[0]);
            Arguments = (CallArgumentListNode) AddChild("argument list", treeNode.ChildNodes[1]);
        }
    }
}
