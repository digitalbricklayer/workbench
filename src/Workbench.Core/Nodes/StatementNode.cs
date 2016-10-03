using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class StatementNode : AstNode
    {
        public AstNode InnerStatement { get; private set; }
        public bool IsIfStatement => InnerStatement is IfStatementNode;
        public bool IsBindingStatement => InnerStatement is CallStatementNode;
        public CallStatementNode BindingStatementNode => InnerStatement as CallStatementNode;
        public IfStatementNode IfStatementNode => InnerStatement as IfStatementNode;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            InnerStatement = AddChild("inner statement", treeNode.ChildNodes[0]);
        }
    }
}