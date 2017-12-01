using System.Diagnostics;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class VisualizerExpressionNode : AstNode
    {
        public AstNode InnerExpression { get; private set; }

        public bool IsLiteral => InnerExpression is NumberLiteralNode;

        public bool IsExpression => IsValueReferenceExpression || IsCounterReferenceExpression;

        public bool IsValueReferenceExpression => InnerExpression is ValueReferenceStatementNode;

        public bool IsCounterReferenceExpression
        {
            get
            {
                return InnerExpression is CounterReferenceNode;
            }
        }

        public ValueReferenceStatementNode ValueReference => InnerExpression as ValueReferenceStatementNode;
        public CounterReferenceNode CounterReference
        {
            get
            {
                var x = (CounterReferenceNode)InnerExpression;
                return x;
            }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            InnerExpression = AddChild("Inner", treeNode.ChildNodes[0]);
        }

        public int GetLiteral()
        {
            Debug.Assert(IsLiteral);
            var literalNode = InnerExpression as NumberLiteralNode;
            Debug.Assert(literalNode != null);
            return literalNode.Value;
        }
    }
}
