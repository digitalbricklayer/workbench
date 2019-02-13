using System.Diagnostics.Contracts;
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
                Contract.Assume(IsLiteral);
                return (IntegerLiteralNode) InnerExpression;
            }
        }

        public CounterReferenceNode CounterReference
        {
            get
            {
                Contract.Assume(IsCounterReference);
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
                Contract.Assume(IsFunctionInvocation);
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
