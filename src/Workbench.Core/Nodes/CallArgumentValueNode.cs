using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class CallArgumentValueNode : AstNode
    {
        public AstNode Value { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Value = AddChild("value", treeNode.ChildNodes[0]);
        }
    }
}
