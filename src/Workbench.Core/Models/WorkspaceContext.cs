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
            var newVariable = new VariableGraphicModel(theVariableName, theDomainExpression);
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
            var newVariable = new AggregateVariableGraphicModel(newAggregateName, aggregateSize, newDomainExpression);
            this.workspace.Model.AddVariable(newVariable);

            return this;
        }

        public WorkspaceContext WithSharedDomain(string newDomainName, string newDomainExpression)
        {
            var newDomain = new DomainGraphicModel(newDomainName, new Point(1, 1),  new DomainModel(newDomainExpression));
            this.workspace.Model.AddSharedDomain(newDomain);

            return this;
        }

        public WorkspaceContext WithSharedDomain(DomainGraphicModel theDomainExpression)
        {
            this.workspace.Model.AddSharedDomain(theDomainExpression);
            return this;
        }

        public WorkspaceContext WithConstraintExpression(string theConstraintExpression)
        {
            var theConstraintModel = new ExpressionConstraintModel(theConstraintExpression);
            this.workspace.Model.AddConstraint(new ExpressionConstraintGraphicModel("Constraint 1", new Point(0, 0), theConstraintModel));
            return this;
        }

        public WorkspaceContext WithConstraintAllDifferent(string theExpression)
        {
            var newConstraintModel = new AllDifferentConstraintModel(new AllDifferentConstraintExpressionModel(theExpression));
            this.workspace.Model.AddConstraint(new AllDifferentConstraintGraphicModel("All different", new Point(0, 0), newConstraintModel));
            return this;
        }

        public WorkspaceContext WithChessboardVisualizer(string theVisualizerName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVisualizerName));

            var theVisualizer = new ChessboardVisualizerModel(theVisualizerName, new Point());
            this.workspace.AddVisualizer(theVisualizer);
            return this;
        }

        public WorkspaceContext WithVisualizerBinding(string theBindingExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theBindingExpression));

            this.workspace.AddSolutionBinding(new VisualizerBindingExpressionModel(theBindingExpression));
            return this;
        }

        public WorkspaceContext WithGridVisualizer(GridVisualizerModel theVisualizer)
        {
            Contract.Requires<ArgumentException>(theVisualizer != null);

            this.workspace.AddVisualizer(theVisualizer);
            return this;
        }

        public WorkspaceContext WithGridVisualizer(string theVisualizerName, params string[] columnNames)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVisualizerName));

            var theVisualizer = new GridVisualizerModel(theVisualizerName, new Point());
            foreach (var columnName in columnNames)
            {
                theVisualizer.AddColumn(new GridColumnModel(columnName));
            }
            this.workspace.AddVisualizer(theVisualizer);
            return this;
        }

        public WorkspaceContext WithGridVisualizer(string theVisualizerName, string[] columnNames, GridRowModel[] rows)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVisualizerName));
            Contract.Requires<ArgumentNullException>(columnNames != null);
            Contract.Requires<ArgumentNullException>(rows != null);

            var theVisualizer = new GridVisualizerModel(theVisualizerName, new Point());
            foreach (var columnName in columnNames)
            {
                theVisualizer.AddColumn(new GridColumnModel(columnName));
            }
            foreach (var row in rows)
            {
                theVisualizer.AddRow(row);
            }
            this.workspace.AddVisualizer(theVisualizer);

            return this;
        }

        public ModelModel GetModel()
        {
            Contract.Assume(this.workspace != null);
            return this.workspace.Model;
        }

        public DisplayModel GetDisplay()
        {
            Contract.Assume(this.workspace != null);
            return this.workspace.Solution.Display;
        }

        public WorkspaceModel Build()
        {
            Contract.Ensures(Contract.Result<WorkspaceModel>() != null);
            return this.workspace;
        }
    }
}