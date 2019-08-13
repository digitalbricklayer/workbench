using System.Diagnostics;
using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter.Ast;

namespace Workbench.Core.Nodes
{
    public class SubscriptStatementNode : AstNode
    {
        public AstNode InnerExpression { get; private set; }

        public bool IsSubscript => InnerExpression is SubscriptNode;

        public bool IsCounterReference => InnerExpression is CounterReferenceNode;

        public int Subscript
        {
            get
            {
                Debug.Assert(IsSubscript);
                var subscriptNode = (SubscriptNode) InnerExpression;
                return subscriptNode.Subscript;
            }
        }

        public CounterReferenceNode CounterReference => InnerExpression as CounterReferenceNode;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            InnerExpression = AddChild("Inner", treeNode.ChildNodes[0]);
        }
    }
}
