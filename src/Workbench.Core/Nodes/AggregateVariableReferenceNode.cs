using System;
using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class AggregateVariableReferenceNode : ConstraintExpressionBaseNode
    {
        public string VariableName { get; private set; }
        public int Subscript { get; private set; }

        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            VariableName = treeNode.ChildNodes[0].FindTokenAndGetText();
            // Child nodes 1 and 3 contain the opening and closing brackets
            Subscript = int.Parse(treeNode.ChildNodes[2].FindTokenAndGetText());
        }
    }
}
