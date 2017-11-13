using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Workbench.Core.Nodes
{
    public class ItemsListNode : AstNode
    {
        private readonly IList<ItemNameNode> items;

        public ItemsListNode()
        {
            this.items = new List<ItemNameNode>();
        }

        public IReadOnlyList<ItemNameNode> Values
        {
            get { return new ReadOnlyCollection<ItemNameNode>(this.items); }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            foreach (var node in treeNode.ChildNodes)
            {
                var newItem = (ItemNameNode) AddChild("item", node);
                this.items.Add(newItem);
            }
        }
    }
}
