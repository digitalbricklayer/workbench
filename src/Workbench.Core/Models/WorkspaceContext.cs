using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Fluent interface for building workspaces.
    /// </summary>
    public class WorkspaceContext
    {
        private readonly WorkspaceModel workspace;

        /// <summary>
        /// Initialize a workspace context with a workspace model.
        /// </summary>
        /// <param name="theWorkspace"></param>
        internal WorkspaceContext(WorkspaceModel theWorkspace)
        {
            if (theWorkspace == null)
                throw new ArgumentNullException(nameof(theWorkspace));
            this.workspace = theWorkspace;
        }

        /// <summary>
        /// Add a singleton variable.
        /// </summary>
        /// <param name="theVariableName">Variable name.</param>
        /// <param name="theDomainExpression">Variable domain.</param>
        /// <returns>Workspace context.</returns>
        public WorkspaceContext AddSingleton(string theVariableName, string theDomainExpression)
        {
            var newVariable = new SingletonVariableModel(this.workspace.Model, new ModelName(theVariableName), new VariableDomainExpressionModel(theDomainExpression));
            this.workspace.Model.AddVariable(newVariable);

            return this;
        }

        /// <summary>
        /// Add an aggregate variable.
        /// </summary>
        /// <param name="newAggregateName">Variable name.</param>
        /// <param name="aggregateSize">Size.</param>
        /// <param name="newDomainExpression">Variable domain.</param>
        /// <returns>Workspace context.</returns>
        public WorkspaceContext AddAggregate(string newAggregateName, int aggregateSize, string newDomainExpression)
        {
            var newVariable = new AggregateVariableModel(this.workspace.Model, new ModelName(newAggregateName), aggregateSize, new VariableDomainExpressionModel(newDomainExpression));
            this.workspace.Model.AddVariable(newVariable);

            return this;
        }

        public WorkspaceContext WithSharedDomain(string newDomainName, string newDomainExpression)
        {
            var newDomain = new DomainModel(new ModelName(newDomainName), new DomainExpressionModel(newDomainExpression));
            this.workspace.Model.AddSharedDomain(newDomain);

            return this;
        }

        public WorkspaceContext WithSharedDomain(DomainModel theDomain)
        {
            this.workspace.Model.AddSharedDomain(theDomain);
            return this;
        }

        public WorkspaceContext WithConstraintExpression(string theConstraintExpression)
        {
            var theConstraintModel = new ExpressionConstraintModel(new ConstraintExpressionModel(theConstraintExpression));
            this.workspace.Model.AddConstraint(theConstraintModel);
            return this;
        }

        public WorkspaceContext WithConstraintAllDifferent(string theExpression)
        {
            var newConstraint = new AllDifferentConstraintModel(new AllDifferentConstraintExpressionModel(theExpression));
            this.workspace.Model.AddConstraint(newConstraint);
            return this;
        }

        public WorkspaceContext WithChessboardVisualizer(string theVisualizerName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVisualizerName));

            var theChessboard = new ChessboardModel(new ModelName(theVisualizerName));
            var theChessboardVisualizer = new ChessboardVisualizerModel(theChessboard, new WorkspaceTabTitle());
            this.workspace.AddVisualizer(theChessboardVisualizer);
            return this;
        }

        public WorkspaceContext WithVisualizerBinding(string theBindingExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theBindingExpression));

            this.workspace.AddBindingExpression(new VisualizerBindingExpressionModel(theBindingExpression));
            return this;
        }

        public WorkspaceContext WithGridVisualizer(TableTabModel theTab)
        {
            Contract.Requires<ArgumentException>(theTab != null);

            this.workspace.AddVisualizer(theTab);
            return this;
        }

        public WorkspaceContext WithGridVisualizer(string theVisualizerName, params string[] columnNames)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVisualizerName));

            var tableModel = new TableModel();
            var theTableVisualizer = new TableTabModel(tableModel, new WorkspaceTabTitle(theVisualizerName));
            foreach (var columnName in columnNames)
            {
                theTableVisualizer.AddColumn(new TableColumnModel(columnName));
            }
            this.workspace.AddVisualizer(theTableVisualizer);
            return this;
        }

        public WorkspaceContext WithGridVisualizer(string theVisualizerName, string[] columnNames, TableRowModel[] rows)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVisualizerName));
            Contract.Requires<ArgumentNullException>(columnNames != null);
            Contract.Requires<ArgumentNullException>(rows != null);

            var tableModel = new TableModel();
            var theTableVisualizer = new TableTabModel(tableModel, new WorkspaceTabTitle(theVisualizerName));
            foreach (var columnName in columnNames)
            {
                theTableVisualizer.AddColumn(new TableColumnModel(columnName));
            }
            foreach (var row in rows)
            {
                theTableVisualizer.AddRow(row);
            }
            this.workspace.AddVisualizer(theTableVisualizer);

            return this;
        }

        public WorkspaceModel Build()
        {
            Contract.Ensures(Contract.Result<WorkspaceModel>() != null);
            return this.workspace;
        }
    }
}