using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class SingletonVariableReferenceNode : ConstraintExpressionBaseNode
    {
        public string VariableName { get; private set; }

        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            VariableName = treeNode.ChildNodes[0].FindTokenAndGetText();
        }
    }
}