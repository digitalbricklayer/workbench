using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class TableRangeExpressionNode : AstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            From = (TableCellReferenceNode) AddChild("table cell reference", treeNode.ChildNodes[0]);
            To = (TableCellReferenceNode) AddChild("table cell reference", treeNode.ChildNodes[1]);
        }

        /// <summary>
        /// Gets the from part of the range.
        /// </summary>
        public TableCellReferenceNode From { get; private set; }

        /// <summary>
        /// Gets the to part of the range.
        /// </summary>
        public TableCellReferenceNode To { get; private set; }
    }
}