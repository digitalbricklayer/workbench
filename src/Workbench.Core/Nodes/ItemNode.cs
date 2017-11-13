using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class ItemNode : AstNode
    {
        public ItemNameNode Name { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Name = (ItemNameNode)AddChild("name", treeNode.ChildNodes[0]);
        }
    }
}
