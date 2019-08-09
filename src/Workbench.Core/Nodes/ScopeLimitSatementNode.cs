using System.Diagnostics;
using System.Diagnostics.Contracts;
using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter.Ast;

namespace Workbench.Core.Nodes
{
    public class ScopeLimitSatementNode : AstNode
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
                return (CounterReferenceNode) InnerExpression;
            }
        }

        public bool IsLiteral
        {
            get
            {
                var literal = InnerExpression as IntegerLiteralNode;
                return literal != null;
            }
        }

        public bool IsCounterReference
        {
            get
            {
                var counterReference = InnerExpression as CounterReferenceNode;
                return counterReference != null;
            }
        }

        public bool IsFunctionInvocation
        {
            get
            {
                var functionInvocation = InnerExpression as FunctionInvocationNode;
                return functionInvocation != null;
            }
        }

        public FunctionInvocationNode FunctionInvocation
        {
            get
            {
                Debug.Assert(IsFunctionInvocation);
                return (FunctionInvocationNode)InnerExpression;
            }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            // Can be either a literal, a counter reference or function call...
            InnerExpression = AddChild("Inner", treeNode.ChildNodes[0]);
        }
    }
}
