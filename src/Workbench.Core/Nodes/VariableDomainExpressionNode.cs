﻿using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    /// <summary>
    /// Root node for the variable domain expression.
    /// </summary>
    public class VariableDomainExpressionNode : AstNode
    {
        public AstNode Inner { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Inner = AddChild("inner", treeNode.ChildNodes[0]);
        }
    }
}