using System;
using Workbench.Core.Models;

namespace Workbench.Core
{
    /// <summary>
    /// Builder for creating a workspace.
    /// </summary>
    public sealed class WorkspaceBuilder
    {
        private readonly WorkspaceModel _workspace;
        private readonly BundleModel _model;

        /// <summary>
        /// Initialize a workspace builder with a model name.
        /// </summary>
        public WorkspaceBuilder(string theModelName)
        {
            if (string.IsNullOrWhiteSpace(theModelName))
                throw new ArgumentException(nameof(theModelName));

            _workspace = new WorkspaceModel(new ModelName(theModelName));
            _model = _workspace.Model;
        }

        /// <summary>
        /// Initialize a workspace builder with a model name.
        /// </summary>
        public WorkspaceBuilder(ModelName theModelName)
        {
            _workspace = new WorkspaceModel(theModelName);
            _model = _workspace.Model;
        }

        /// <summary>
        /// Initialize a workspace builder with default values.
        /// </summary>
        public WorkspaceBuilder()
        {
            _workspace = new WorkspaceModel();
            _model = _workspace.Model;
        }

        /// <summary>
        /// Add a singleton variable.
        /// </summary>
        /// <param name="theVariableName">Variable name.</param>
        /// <param name="theDomainExpression">Variable domain.</param>
        /// <returns>Workspace context.</returns>
        public WorkspaceBuilder AddSingleton(string theVariableName, string theDomainExpression)
        {
            if (string.IsNullOrWhiteSpace(theVariableName))
                throw new ArgumentException(nameof(theVariableName));

            if (string.IsNullOrWhiteSpace(theDomainExpression))
                throw new ArgumentException(nameof(theDomainExpression));

            var newVariable = new SingletonVariableModel(_model, new ModelName(theVariableName), new InlineDomainModel(theDomainExpression));
            _model.AddVariable(newVariable);

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
            if (string.IsNullOrWhiteSpace(newAggregateName))
                throw new ArgumentException(nameof(newAggregateName));

            if (aggregateSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(aggregateSize));

            if (string.IsNullOrWhiteSpace(newDomainExpression))
                throw new ArgumentException(nameof(newDomainExpression));

            var newVariable = new AggregateVariableModel(_model, new ModelName(newAggregateName), aggregateSize, new InlineDomainModel(newDomainExpression));
            _model.AddVariable(newVariable);

            return this;
        }

        /// <summary>
        /// Add an aggregate variable.
        /// </summary>
        /// <param name="action">User supplied action.</param>
        /// <returns>Workspace context.</returns>
        public WorkspaceBuilder AddAggregate(Action<AggregateVariableConfiguration> action)
        {
            var variableConfig = CreateDefaultAggregateVariableConfig();

            action(variableConfig);

            var newVariable = variableConfig.Build();
            _model.AddVariable(newVariable);

            return this;
        }

        public WorkspaceBuilder WithSharedDomain(string newDomainName, string newDomainExpression)
        {
            if (string.IsNullOrWhiteSpace(newDomainName))
                throw new ArgumentException(nameof(newDomainName));

            if (string.IsNullOrWhiteSpace(newDomainExpression))
                throw new ArgumentException(nameof(newDomainExpression));

            var newDomain = new SharedDomainModel(_model, new ModelName(newDomainName), new SharedDomainExpressionModel(newDomainExpression));
            _model.AddSharedDomain(newDomain);

            return this;
        }

        public WorkspaceBuilder WithConstraintExpression(string theConstraintExpression)
        {
            if (string.IsNullOrWhiteSpace(theConstraintExpression))
                throw new ArgumentException(nameof(theConstraintExpression));

            var constraintModel = new ExpressionConstraintModel(_model, new ConstraintExpressionModel(theConstraintExpression));

            _model.AddConstraint(constraintModel);
            return this;
        }

        public WorkspaceBuilder WithConstraintAllDifferent(string theExpression)
        {
            if (string.IsNullOrWhiteSpace(theExpression))
                throw new ArgumentException(nameof(theExpression));

            var newConstraint = new AllDifferentConstraintModel(_model, new AllDifferentConstraintExpressionModel(theExpression));
            _model.AddConstraint(newConstraint);

            return this;
        }

        public WorkspaceBuilder WithChessboard(string theVisualizerName)
        {
            if (string.IsNullOrWhiteSpace(theVisualizerName))
                throw new ArgumentException(nameof(theVisualizerName));

            var chessboard = new ChessboardModel(new ModelName(theVisualizerName));
            var chessboardVisualizer = new ChessboardTabModel(chessboard, new WorkspaceTabTitle());
            _workspace.AddVisualizer(chessboardVisualizer);
            return this;
        }

        public WorkspaceBuilder WithBinding(string theBindingExpression)
        {
            if (string.IsNullOrWhiteSpace(theBindingExpression))
                throw new ArgumentException(nameof(theBindingExpression));

            _workspace.AddBindingExpression(new VisualizerBindingExpressionModel(theBindingExpression));
            return this;
        }

        public WorkspaceBuilder WithTable(TableTabModel theTab)
        {
            _workspace.AddVisualizer(theTab);
            return this;
        }

        public WorkspaceBuilder AddBundle(Action<BundleConfiguration> action)
        {
            var bundleConfiguration = new BundleConfiguration(_workspace);

            action(bundleConfiguration);

            var newBundle = bundleConfiguration.Build();
            _model.AddBundle(newBundle);

            return this;
        }

        private AggregateVariableConfiguration CreateDefaultAggregateVariableConfig()
        {
            return new AggregateVariableConfiguration(_workspace);
        }

        public WorkspaceBuilder AddBucket(Action<BucketConfiguration> action)
        {
            var bucketConfiguration = new BucketConfiguration(_workspace);

            action(bucketConfiguration);

            var newBucket = bucketConfiguration.Build();
            _model.AddBucket(newBucket);

            return this;
        }

        public WorkspaceModel Build()
        {
            return _workspace;
        }
    }
}
