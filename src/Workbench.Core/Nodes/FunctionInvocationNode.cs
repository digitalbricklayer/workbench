using System;
using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter.Ast;

namespace Workbench.Core.Nodes
{
    public class FunctionInvocationNode : AstNode
    {
        public string FunctionName { get; private set; }
        public FunctionArgumentListNode ArgumentList { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            FunctionName = Convert.ToString(treeNode.ChildNodes[0].Token.Value);
            ArgumentList = (FunctionArgumentListNode) AddChild("arguments", treeNode.ChildNodes[1]);
        }

#if false
        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            throw new NotImplementedException();
        }
#endif
    }
}