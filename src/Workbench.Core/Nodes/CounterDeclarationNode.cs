using System;
using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter.Ast;

namespace Workbench.Core.Nodes
{
    public class CounterDeclarationNode : AstNode
    {
        public string CounterName { get; private set; }

#if false
        /// <summary>
        /// Accept a visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
#endif

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            CounterName = Convert.ToString(treeNode.Token.Value);
        }
    }
}