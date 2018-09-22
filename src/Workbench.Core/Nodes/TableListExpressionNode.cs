using System.Collections.Generic;
using System.Collections.ObjectModel;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class TableListExpressionNode : AstNode
    {
        private readonly IList<TableCellReferenceNode> _statements;

        public TableListExpressionNode()
        {
            _statements = new List<TableCellReferenceNode>();
        }

        public IReadOnlyCollection<TableCellReferenceNode> Statements => new ReadOnlyCollection<TableCellReferenceNode>(_statements);

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            foreach (var node in treeNode.ChildNodes)
            {
                var newCellReference = (TableCellReferenceNode)AddChild("cell reference", node);
                _statements.Add(newCellReference);
            }
        }
    }
}