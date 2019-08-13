using System.Diagnostics;
using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter.Ast;

namespace Workbench.Core.Nodes
{
    public class ExpanderCountNode : AstNode
    {
        public AstNode InnerExpression { get; private set; }

        public IntegerLiteralNode Literal
        {
            get
            {
                Debug.Assert(IsLiteral);
                return (IntegerLiteralNode) InnerExpression;
            }
        }

        public CounterReferenceNode CounterReference
        {
            get
            {
                Debug.Assert(IsCounterReference);
                return (CounterReferenceNode)InnerExpression;
            }
        }

        public bool IsLiteral
        {
            get
            {
                return InnerExpression is IntegerLiteralNode;
            }
        }

        public bool IsCounterReference
        {
            get
            {
                return InnerExpression is CounterReferenceNode;
            }
        }

        public bool IsFunctionInvocation
        {
            get
            {
                return InnerExpression is FunctionInvocationNode;
            }
        }

        public FunctionInvocationNode FunctionInvocation
        {
            get
            {
                Debug.Assert(IsFunctionInvocation);
                return (FunctionInvocationNode) InnerExpression;
            }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            // Can be either a literal or a counter reference...
            InnerExpression = AddChild("Inner", treeNode.ChildNodes[0]);
        }
    }
}
