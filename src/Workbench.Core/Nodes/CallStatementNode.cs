using System.Linq;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class CallStatementNode : AstNode
    {
        public VisualizerNameReferenceNode Name { get; private set; }
        public CallArgumentNodeList Arguments { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            // The expander statement can be empty and still valid...
            if (!treeNode.ChildNodes.Any()) return;
            Name = (VisualizerNameReferenceNode) AddChild("visualizer name", treeNode.ChildNodes[0]);
            Arguments = (CallArgumentNodeList) AddChild("argument list", treeNode.ChildNodes[2]);
        }
    }
}
