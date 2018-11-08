using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core
{
    /// <summary>
    /// Builder for creating a workspace.
    /// </summary>
    public class WorkspaceBuilder
    {
        private readonly WorkspaceModel _workspace;

        /// <summary>
        /// Initialize a workspace builder with a model name.
        /// </summary>
        public WorkspaceBuilder(string theModelName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theModelName));
            this._workspace = new WorkspaceModel(new ModelName(theModelName));
        }

        /// <summary>
        /// Initialize a workspace builder with a model name.
        /// </summary>
        public WorkspaceBuilder(ModelName theModelName)
        {
            Contract.Requires<ArgumentNullException>(theModelName != null);
            this._workspace = new WorkspaceModel(theModelName);
        }

        /// <summary>
        /// Initialize a workspace builder with default values.
        /// </summary>
        public WorkspaceBuilder()
        {
            this._workspace = new WorkspaceModel();
        }

        /// <summary>
        /// Add a singleton variable.
        /// </summary>
        /// <param name="theVariableName">Variable name.</param>
        /// <param name="theDomainExpression">Variable domain.</param>
        /// <returns>Workspace context.</returns>
        public WorkspaceBuilder AddSingleton(string theVariableName, string theDomainExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVariableName));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theDomainExpression));
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);
            var newVariable = new SingletonVariableModel(this._workspace.Model, new ModelName(theVariableName), new InlineDomainModel(theDomainExpression));
            this._workspace.Model.AddVariable(newVariable);

            return this;
        }

        /// <summary>
        /// Add an aggregate variable.
        /// </summary>
        /// <param name="newAggregateName">Variable name.</param>
        /// <param name="aggregateSize">Size.</param>
        /// <param name="newDomainExpression">Variable domain.</param>
        /// <returns>Workspace context.</returns>
        public WorkspaceBuilder AddAggregate(string newAggregateName, int aggregateSize, string newDomainExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(newAggregateName));
            Contract.Requires<ArgumentOutOfRangeException>(aggregateSize > 0);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(newDomainExpression));
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);
            var newVariable = new AggregateVariableModel(this._workspace.Model.Workspace, new ModelName(newAggregateName), aggregateSize, new InlineDomainModel(newDomainExpression));
            this._workspace.Model.AddVariable(newVariable);

            return this;
        }

        public WorkspaceBuilder WithSharedDomain(string newDomainName, string newDomainExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(newDomainName));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(newDomainExpression));
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);
            var newDomain = new SharedDomainModel(_workspace.Model, new ModelName(newDomainName), new SharedDomainExpressionModel(newDomainExpression));
            this._workspace.Model.AddSharedDomain(newDomain);

            return this;
        }

        public WorkspaceBuilder WithSharedDomain(SharedDomainModel theDomain)
        {
            Contract.Requires<ArgumentNullException>(theDomain != null);
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);
            this._workspace.Model.AddSharedDomain(theDomain);
            return this;
        }

        public WorkspaceBuilder WithConstraintExpression(string theConstraintExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theConstraintExpression));
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);
            var theConstraintModel = new ExpressionConstraintModel(_workspace.Model, new ConstraintExpressionModel(theConstraintExpression));
            this._workspace.Model.AddConstraint(theConstraintModel);
            return this;
        }

        public WorkspaceBuilder WithConstraintAllDifferent(string theExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theExpression));
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);
            var newConstraint = new AllDifferentConstraintModel(_workspace.Model, new AllDifferentConstraintExpressionModel(theExpression));
            this._workspace.Model.AddConstraint(newConstraint);
            return this;
        }

        public WorkspaceBuilder WithChessboard(string theVisualizerName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVisualizerName));
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);

            var theChessboard = new ChessboardModel(new ModelName(theVisualizerName));
            var theChessboardVisualizer = new ChessboardTabModel(theChessboard, new WorkspaceTabTitle());
            this._workspace.AddVisualizer(theChessboardVisualizer);
            return this;
        }

        public WorkspaceBuilder WithBinding(string theBindingExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theBindingExpression));
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);

            this._workspace.AddBindingExpression(new VisualizerBindingExpressionModel(theBindingExpression));
            return this;
        }

        public WorkspaceBuilder WithTable(TableTabModel theTab)
        {
            Contract.Requires<ArgumentException>(theTab != null);
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);

            this._workspace.AddVisualizer(theTab);
            return this;
        }

        public WorkspaceModel Build()
        {
            Contract.Ensures(Contract.Result<WorkspaceModel>() != null);
            return this._workspace;
        }
    }
}