using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class TableRangeNode : AstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            TableReferenceNode = (TableReferenceNode) AddChild("table name", treeNode.ChildNodes[0]);
            RowIndex = (NumberLiteralNode) AddChild("row index", treeNode.ChildNodes[1]);
            ColumnIndex = (NumberLiteralNode)AddChild("column index", treeNode.ChildNodes[2]);
        }

        /// <summary>
        /// Gets the table reference node.
        /// </summary>
        public TableReferenceNode TableReferenceNode { get; private set; }

        /// <summary>
        /// Gets the column index node.
        /// </summary>
        public NumberLiteralNode ColumnIndex { get; private set; }

        /// <summary>
        /// Gets the row index node.
        /// </summary>
        public NumberLiteralNode RowIndex { get; private set; }
    }
}