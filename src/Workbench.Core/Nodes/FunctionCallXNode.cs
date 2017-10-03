using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class FunctionCallXNode : AstNode
    {
        public FunctionNameNode FunctionName { get; private set; }
        public FunctionArgumentListNode ArgumentList { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            FunctionName = (FunctionNameNode) AddChild("function name", treeNode.ChildNodes[0]);
            ArgumentList = (FunctionArgumentListNode) AddChild("arguments", treeNode.ChildNodes[1]);
        }
    }
}