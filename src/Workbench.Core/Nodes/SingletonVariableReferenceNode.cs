using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class SingletonVariableReferenceNode : AstNode
    {
        public string VariableName { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var variableNameWithPrefix = treeNode.ChildNodes[0].FindTokenAndGetText();
            VariableName = variableNameWithPrefix.Substring(1);
        }
    }
}