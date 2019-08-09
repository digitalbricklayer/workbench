using System;
using System.Collections.Generic;
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
            if (string.IsNullOrWhiteSpace(theModelName))
                throw new ArgumentException(nameof(theModelName));
            _workspace = new WorkspaceModel(new ModelName(theModelName));
        }

        /// <summary>
        /// Initialize a workspace builder with a model name.
        /// </summary>
        public WorkspaceBuilder(ModelName theModelName)
        {
            _workspace = new WorkspaceModel(theModelName);
        }

        /// <summary>
        /// Initialize a workspace builder with default values.
        /// </summary>
        public WorkspaceBuilder()
        {
            _workspace = new WorkspaceModel();
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

            var newVariable = new SingletonVariableModel(_workspace.Model, new ModelName(theVariableName), new InlineDomainModel(theDomainExpression));
            _workspace.Model.AddVariable(newVariable);

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

            var newVariable = new AggregateVariableModel(_workspace.Model.Workspace, new ModelName(newAggregateName), aggregateSize, new InlineDomainModel(newDomainExpression));
            _workspace.Model.AddVariable(newVariable);

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
            _workspace.Model.AddVariable(newVariable);

            return this;
        }

        public WorkspaceBuilder WithSharedDomain(string newDomainName, string newDomainExpression)
        {
            if (string.IsNullOrWhiteSpace(newDomainName))
                throw new ArgumentException(nameof(newDomainName));

            if (string.IsNullOrWhiteSpace(newDomainExpression))
                throw new ArgumentException(nameof(newDomainExpression));

            var newDomain = new SharedDomainModel(_workspace.Model, new ModelName(newDomainName), new SharedDomainExpressionModel(newDomainExpression));
            _workspace.Model.AddSharedDomain(newDomain);

            return this;
        }

        public WorkspaceBuilder WithConstraintExpression(string theConstraintExpression)
        {
            if (string.IsNullOrWhiteSpace(theConstraintExpression))
                throw new ArgumentException(nameof(theConstraintExpression));

            var constraintModel = new ExpressionConstraintModel(_workspace.Model, new ConstraintExpressionModel(theConstraintExpression));

            _workspace.Model.AddConstraint(constraintModel);
            return this;
        }

        public WorkspaceBuilder WithConstraintAllDifferent(string theExpression)
        {
            if (string.IsNullOrWhiteSpace(theExpression))
                throw new ArgumentException(nameof(theExpression));

            var newConstraint = new AllDifferentConstraintModel(_workspace.Model, new AllDifferentConstraintExpressionModel(theExpression));
            _workspace.Model.AddConstraint(newConstraint);

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
            _workspace.Model.AddBundle(newBundle);

            return this;
        }

        private AggregateVariableConfiguration CreateDefaultAggregateVariableConfig()
        {
            return new AggregateVariableConfiguration(_workspace);
        }

        public WorkspaceBuilder AddBucket(Action<BucketConfiguration> action)
        {
            var bundleConfiguration = new BucketConfiguration(_workspace);

            action(bundleConfiguration);

            var newBundle = bundleConfiguration.Build();
            _workspace.Model.AddBucket(newBundle);

            return this;
        }

        public WorkspaceModel Build()
        {
            return _workspace;
        }
    }

    public sealed class BucketConfiguration
    {
        private string _name;
        private string _bundleName;
        private int _size;
        private readonly WorkspaceModel _workspace;

        public BucketConfiguration(WorkspaceModel workspace)
        {
            _workspace = workspace;
            _name = string.Empty;
            _bundleName = string.Empty;
            _size = 1;
        }

        public BucketConfiguration WithName(string name)
        {
            _name = name;
            return this;
        }

        public BucketConfiguration WithSize(int size)
        {
            _size = size;
            return this;
        }

        public BucketConfiguration WithContents(string bundleName)
        {
            _bundleName = bundleName;
            return this;
        }

        public BucketVariableModel Build()
        {
            var bundle = _workspace.Model.GetBundleByName(_bundleName);
            return new BucketVariableModel(_workspace, new ModelName(_name), _size, bundle);
        }
    }

    public sealed class BundleConfiguration
    {
        private string _name;
        private readonly List<SingletonVariableModel> _singletons;
        private readonly List<ConstraintModel> _constraints;
        private readonly WorkspaceModel _workspace;

        public BundleConfiguration(WorkspaceModel workspace)
        {
            _name = string.Empty;
            _workspace = workspace;
            _singletons = new List<SingletonVariableModel>();
            _constraints = new List<ConstraintModel>();
        }

        public BundleConfiguration WithName(string bundleName)
        {
            if (string.IsNullOrWhiteSpace(bundleName))
                throw new ArgumentException(nameof(bundleName));
            _name = bundleName;
            return this;
        }

        public BundleConfiguration WithAllDifferentConstraint(string expression)
        {
            _constraints.Add(new AllDifferentConstraintModel(_workspace.Model, new AllDifferentConstraintExpressionModel(expression)));
            return this;
        }

        public BundleConfiguration AddSingleton(string variableName, string domainExpression)
        {
            _singletons.Add(new SingletonVariableModel(_workspace.Model, new ModelName(variableName), new InlineDomainModel(domainExpression)));
            return this;
        }

        public BundleModel Build()
        {
            var newBundle = new BundleModel(new ModelName(_name));
            AddSingletons(newBundle);
            AddConstraints(newBundle);

            return newBundle;
        }

        private void AddConstraints(BundleModel bundle)
        {
            foreach (var constraint in _constraints)
            {
                switch (constraint)
                {
                    case AllDifferentConstraintModel allDifferentConstraint:
                        bundle.AddAllDifferentConstraint(allDifferentConstraint);
                        break;
                }
            }
        }

        private void AddSingletons(BundleModel bundle)
        {
            foreach (var singleton in _singletons)
            {
                bundle.AddSingleton(singleton);
            }
        }
    }

    public sealed class AggregateVariableConfiguration
    {
        private string _name;
        private int _size;
        private string _domainExpression;
        private readonly WorkspaceModel _workspace;

        public AggregateVariableConfiguration(WorkspaceModel workspace)
        {
            _workspace = workspace;
        }

        public void WithName(string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
                throw new ArgumentException(nameof(variableName));
            _name = variableName;
        }

        public void WithSize(int variableSize)
        {
            if (variableSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(variableSize));
            _size = variableSize;
        }

        public void WithDomain(string domainExpression)
        {
            if (string.IsNullOrWhiteSpace(domainExpression))
                throw new ArgumentException(nameof(domainExpression));
            _domainExpression = domainExpression;
        }

        public AggregateVariableModel Build()
        {
            return new AggregateVariableModel(_workspace, new ModelName(_name), _size, new InlineDomainModel(_domainExpression));
        }
    }
}
