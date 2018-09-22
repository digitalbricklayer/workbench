using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class TableExpressionNode : AstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            TableReferenceNode = (TableReferenceNode) AddChild("table name", treeNode.ChildNodes[0]);
            InnerExpression = AddChild("cell expression", treeNode.ChildNodes[1]);
        }

        /// <summary>
        /// Gets the table reference node.
        /// </summary>
        public TableReferenceNode TableReferenceNode { get; private set; }

        /// <summary>
        /// Gets the range or list expression.
        /// </summary>
        public AstNode InnerExpression { get; set; }
    }
}