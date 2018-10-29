using System.Collections.Generic;
using System.Collections.ObjectModel;
using Irony.Interpreter.Ast;
using Workbench.Core.Nodes;

namespace Workbench.Core
{
    /// <summary>
    /// Capture all table cell reference nodes in an Abstract Syntax Tree.
    /// </summary>
    public class TableCellReferenceCaptureVisitor : IAstVisitor
    {
        private readonly List<TableReferenceNode> _references;

        public TableCellReferenceCaptureVisitor()
        {
            _references = new List<TableReferenceNode>();
        }
        
        public void BeginVisit(IVisitableNode node)
        {
            switch (node)
            {
                case TableReferenceNode tableReference:
                    _references.Add(tableReference);
                    break;
            }
        }

        public void EndVisit(IVisitableNode node)
        {
        }

        public IReadOnlyCollection<TableReferenceNode> GetReferences()
        {
            return new ReadOnlyCollection<TableReferenceNode>(_references);
        }
    }
}