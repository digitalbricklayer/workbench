using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class InfixStatementNode : AstNode
    {
        public AstNode InnerExpression { get; private set; }

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

        public IntegerLiteralNode Literal
        {
            get
            {
                var literalNode = (IntegerLiteralNode) InnerExpression;
                return literalNode;
            }
        }

        public CounterReferenceNode CounterReference
        {
            get
            {
                var counterReference = (CounterReferenceNode) InnerExpression;
                return counterReference;
            }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            InnerExpression = AddChild("Inner", treeNode.ChildNodes[0]);
        }
    }
}
