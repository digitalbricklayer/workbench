using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Workbench.Core.Nodes
{
    public class ListDomainExpressionNode : AstNode
    {
        public ItemsListNode Items { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Items = (ItemsListNode) AddChild("items", treeNode.ChildNodes[0]);
        }
    }
}
