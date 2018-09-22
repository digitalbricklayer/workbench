using System;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class TableCellReferenceNode : AstNode
    {
        public string Expression { get; private set; }

        public TableCellReferenceNode()
        {
            Expression = string.Empty;
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Expression = Convert.ToString(treeNode.Token.Value);
        }
    }
}