using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Evaluator for domain expressions.
    /// </summary>
    internal sealed class DomainExpressionEvaluator
    {
        /// <summary>
        /// Evaluate a range domain expression.
        /// </summary>
        /// <param name="theExpressionNode">The range expression node.</param>
        /// <param name="theModel">The model.</param>
        /// <returns>Domain value.</returns>
        internal static DomainValue Evaluate(RangeDomainExpressionNode theExpressionNode, ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theExpressionNode != null);
            Contract.Requires<ArgumentNullException>(theModel != null);

            var evaluator = new DomainExpressionEvaluator();
            return evaluator.EvaluateNode(theExpressionNode, theModel);
        }

        /// <summary>
        /// Evaluate a list domain expression.
        /// </summary>
        /// <param name="theExpressionNode">List domain expression node.</param>
        /// <returns>Domain value.</returns>
        internal static DomainValue Evaluate(ListDomainExpressionNode theExpressionNode)
        {
            Contract.Requires<ArgumentNullException>(theExpressionNode != null);

            var evaluator = new DomainExpressionEvaluator();
            return evaluator.EvaluateNode(theExpressionNode);
        }

        /// <summary>
        /// Evaluate a shared domain reference.
        /// </summary>
        /// <param name="theExpressionNode">Shared domain reference node.</param>
        /// <param name="theModel">The model.</param>
        /// <returns>Domain value.</returns>
        internal static DomainValue Evaluate(SharedDomainReferenceNode theExpressionNode, ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theExpressionNode != null);
            Contract.Requires<ArgumentNullException>(theModel != null);

            var evaluator = new DomainExpressionEvaluator();
            return evaluator.EvaluateReference(theExpressionNode, theModel);
        }

        /// <summary>
        /// Evaluate a table cell expression.
        /// </summary>
        /// <param name="theExpressionNode">The table cell expression node.</param>
        /// <param name="theWorkspace">The workspace.</param>
        /// <returns>A domain value.</returns>
        internal DomainValue Evaluate(TableExpressionNode theExpressionNode, WorkspaceModel theWorkspace)
        {
            Contract.Requires<ArgumentNullException>(theExpressionNode != null);
            Contract.Requires<ArgumentNullException>(theWorkspace != null);

            Contract.Assume(!string.IsNullOrWhiteSpace(theExpressionNode.TableReferenceNode.Name));

			// The table reference is compulsory
            var theTableTab = (TableTabModel)theWorkspace.GetVisualizerBy(theExpressionNode.TableReferenceNode.Name);
            var theTable = theTableTab.Table;

            switch (theExpressionNode.InnerExpression)
            {
                case TableRangeExpressionNode rangeExpression:
                    return EvaluateRangeExpression(rangeExpression, theTable);

                case TableListExpressionNode listExpression:
                    return EvaluateListExpression(listExpression, theTable);

                default:
                    throw new NotImplementedException("Unknown table expression.");
            }
        }

        private DomainValue EvaluateNode(RangeDomainExpressionNode theExpressionNode, ModelModel theModel)
        {
            var lhsBand = EvaluateBand(theExpressionNode.LeftExpression, theModel);
            var rhsBand = EvaluateBand(theExpressionNode.RightExpression, theModel);
            return new RangeDomainValue(lhsBand, rhsBand, theExpressionNode);
        }

        private DomainValue EvaluateNode(ListDomainExpressionNode theExpressionNode)
        {
            var valueList = new List<string>();
            foreach (var itemNameNode in theExpressionNode.Items.Values)
            {
                valueList.Add(itemNameNode.Value);
            }
            return new ListDomainValue(valueList, theExpressionNode);
        }

        private DomainValue EvaluateReference(SharedDomainReferenceNode theExpressionNode, ModelModel theModel)
        {
            var sharedDomainName = theExpressionNode.DomainName;
            var sharedDomainModel = theModel.GetSharedDomainByName(sharedDomainName.Name);

            var evaluatorContext = new SharedDomainExpressionEvaluatorContext(sharedDomainModel.Expression.Node, theModel);
            return SharedDomainExpressionEvaluator.Evaluate(evaluatorContext);
        }

        private long EvaluateBand(BandExpressionNode theExpression, ModelModel theModel)
        {
            switch (theExpression.Inner)
            {
                case NumberLiteralNode numberLiteral:
                    return numberLiteral.Value;

                case FunctionInvocationNode functionCall:
                    return EvaluateFunction(functionCall, theModel);

                case CharacterLiteralNode characterLiteral:
                    return characterLiteral.Value - 'a' + 1;

                default:
                    throw new NotImplementedException("Unknown band expression node.");
            }
        }

        private long EvaluateFunction(FunctionInvocationNode functionCall, ModelModel theModel)
        {
            Contract.Assert(functionCall.FunctionName == "size", "Only the size function is supporteed at the moment.");

            var variableName = functionCall.ArgumentList.Arguments.First().Value.Value;
            var theVariable = theModel.GetVariableByName(variableName);
            return theVariable.GetSize();
        }

        private DomainValue EvaluateRangeExpression(TableRangeExpressionNode theRangeExpression, TableModel theTable)
        {
            var values = new List<string>();

            if (theRangeExpression.From.Expression == theRangeExpression.To.Expression)
            {
                // A range specifying the name of the column only in both the from and to sides references the entire column
                var columnData = theTable.GetColumnDataByName(theRangeExpression.From.Expression);
                foreach (var aCell in columnData.GetCells())
                {
                    values.Add(aCell.Text);
                }
            }
            else
            {
                // A range specifying a partial column. Assume for now that the range is in the same column.
                var fromRange = ParseCellExpressionText(theRangeExpression.From.Expression);
                var toRange = ParseCellExpressionText(theRangeExpression.To.Expression);
                var columnData = theTable.GetColumnDataByName(fromRange.ColumnName);
                var allCells = columnData.GetCells().ToList();
                foreach (var aCell in allCells.GetRange(fromRange.Index - 1, toRange.Index - fromRange.Index + 1))
                {
                    values.Add(aCell.Text);
                }
            }

            return new TableDomainValue(values);
        }

        private CellInfo ParseCellExpressionText(string fromExpression)
        {
            var index = fromExpression.TrimEnd('1', '2', '3', '4', '5', '6', '7', '8', '9', '0');
            var columnName = Regex.Match(fromExpression, @"\d+$").Value;
            return new CellInfo {ColumnName = index, Index = Convert.ToInt32(columnName)};
        }

        private DomainValue EvaluateListExpression(TableListExpressionNode theListExpression, TableModel theTable)
        {
            var values = new List<string>();

            foreach (var cellReference in theListExpression.Statements)
            {
                var cellInfo = ParseCellExpressionText(cellReference.Expression);
                var columnData = theTable.GetColumnDataByName(cellInfo.ColumnName);
                var x = columnData.GetCellAt(cellInfo.Index);
                values.Add(x.Text);
            }

            return new TableDomainValue(values);
        }
    }

    internal struct CellInfo
    {
        internal string ColumnName { get; set; }

        internal int Index { get; set; }
    }
}