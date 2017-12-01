using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class AggregateVariableReferenceExpressionNode : AstNode
    {
        public AggregateVariableReferenceNode VariableReference { get; private set; }
        public VariableExpressionOperatorType Operator { get; private set; }
        public InfixStatementNode InfixStatement { get; private set; }

#if false
        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
            VariableReference.Accept(visitor);
        }
#endif

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            VariableReference = (AggregateVariableReferenceNode) AddChild("Variable Reference", treeNode.ChildNodes[0]);
            Operator = VariableExpressionOperatorParser.ParseFrom(treeNode.ChildNodes[1].FindTokenAndGetText());
            InfixStatement = (InfixStatementNode) AddChild("Infix Statement", treeNode.ChildNodes[2]);
        }
    }
}
