using System;
using Irony.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class FunctionInvocationNode : ConstraintExpressionBaseNode
    {
        public string FunctionName { get; private set; }
        public FunctionArgumentListNode ArgumentList { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            FunctionName = Convert.ToString(treeNode.ChildNodes[0].Token.Value);
            ArgumentList = (FunctionArgumentListNode) AddChild("arguments", treeNode.ChildNodes[1]);
        }

        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}