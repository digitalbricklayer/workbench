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
            var newVariable = new VariableModel(theVariableName, theDomainExpression);
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
            var newVariable = new AggregateVariableModel(newAggregateName, aggregateSize, newDomainExpression);
            this.workspace.Model.AddVariable(newVariable);

            return this;
        }

        public WorkspaceContext WithSharedDomain(string newDomainName, string newDomainExpression)
        {
            var newDomain = new DomainModel(newDomainName, new Point(1, 1),  new DomainExpressionModel(newDomainExpression));
            this.workspace.Model.AddSharedDomain(newDomain);

            return this;
        }

        public WorkspaceContext WithSharedDomain(DomainModel theDomainExpression)
        {
            this.workspace.Model.AddSharedDomain(theDomainExpression);
            return this;
        }

        public WorkspaceContext WithConstraintExpression(string theConstraintExpression)
        {
            this.workspace.Model.AddConstraint(new ExpressionConstraintModel(theConstraintExpression));
            return this;
        }

        public WorkspaceContext WithConstraintAllDifferent(string theVariableName)
        {
            var variableToBindTo = this.workspace.Model.GetVariableByName(theVariableName);
            this.workspace.Model.AddConstraint(new AllDifferentConstraintModel(variableToBindTo));
            return this;
        }

        public WorkspaceContext WithVariableVisualizerBindingTo(string variableNameToBindTo)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(variableNameToBindTo));
            var theVisualizer = new VariableVisualizerModel(new Point());
            theVisualizer.BindTo(this.workspace.Model.GetVariableByName(variableNameToBindTo));
            this.workspace.AddVisualizer(theVisualizer);
            return this;
        }

        public WorkspaceContext WithChessboardVisualizerBindingTo(string theVisualizerName, string theBindingExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVisualizerName));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theBindingExpression));
            var theVisualizer = new ChessboardVisualizerModel(theVisualizerName, new Point(), theBindingExpression);
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