using System;
using System.Collections.Generic;
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
            _workspace = new WorkspaceModel(new ModelName(theModelName));
        }

        /// <summary>
        /// Initialize a workspace builder with a model name.
        /// </summary>
        public WorkspaceBuilder(ModelName theModelName)
        {
            Contract.Requires<ArgumentNullException>(theModelName != null);
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
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVariableName));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theDomainExpression));
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);
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
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(newAggregateName));
            Contract.Requires<ArgumentOutOfRangeException>(aggregateSize > 0);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(newDomainExpression));
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);
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
            Contract.Requires<ArgumentNullException>(action != null);
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);

            var variableConfig = CreateDefaultAggregateVariableConfig();

            action(variableConfig);

            var newVariable = variableConfig.Build();
            _workspace.Model.AddVariable(newVariable);

            return this;
        }

        public WorkspaceBuilder WithSharedDomain(string newDomainName, string newDomainExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(newDomainName));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(newDomainExpression));
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);
            var newDomain = new SharedDomainModel(_workspace.Model, new ModelName(newDomainName), new SharedDomainExpressionModel(newDomainExpression));
            _workspace.Model.AddSharedDomain(newDomain);

            return this;
        }

        public WorkspaceBuilder WithSharedDomain(SharedDomainModel theDomain)
        {
            Contract.Requires<ArgumentNullException>(theDomain != null);
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);
            _workspace.Model.AddSharedDomain(theDomain);
            return this;
        }

        public WorkspaceBuilder WithConstraintExpression(string theConstraintExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theConstraintExpression));
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);
            var constraintModel = new ExpressionConstraintModel(_workspace.Model, new ConstraintExpressionModel(theConstraintExpression));
            _workspace.Model.AddConstraint(constraintModel);
            return this;
        }

        public WorkspaceBuilder WithConstraintAllDifferent(string theExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theExpression));
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);
            var newConstraint = new AllDifferentConstraintModel(_workspace.Model, new AllDifferentConstraintExpressionModel(theExpression));
            _workspace.Model.AddConstraint(newConstraint);
            return this;
        }

        public WorkspaceBuilder WithChessboard(string theVisualizerName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVisualizerName));
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);

            var chessboard = new ChessboardModel(new ModelName(theVisualizerName));
            var chessboardVisualizer = new ChessboardTabModel(chessboard, new WorkspaceTabTitle());
            _workspace.AddVisualizer(chessboardVisualizer);
            return this;
        }

        public WorkspaceBuilder WithBinding(string theBindingExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theBindingExpression));
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);

            _workspace.AddBindingExpression(new VisualizerBindingExpressionModel(theBindingExpression));
            return this;
        }

        public WorkspaceBuilder WithTable(TableTabModel theTab)
        {
            Contract.Requires<ArgumentException>(theTab != null);
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);

            _workspace.AddVisualizer(theTab);
            return this;
        }

        public WorkspaceBuilder AddBundle(Action<BundleConfiguration> action)
        {
            Contract.Requires<ArgumentNullException>(action != null);
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);

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
            Contract.Requires<ArgumentNullException>(action != null);
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);

            var bundleConfiguration = new BucketConfiguration(_workspace);

            action(bundleConfiguration);

            var newBundle = bundleConfiguration.Build();
            _workspace.Model.AddBucket(newBundle);

            return this;
        }

        public WorkspaceModel Build()
        {
            Contract.Ensures(Contract.Result<WorkspaceModel>() != null);
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
            Contract.Requires<ArgumentNullException>(workspace != null);
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

        public BucketModel Build()
        {
            var bundle = _workspace.Model.GetBundleByName(_bundleName);
            return new BucketModel(new ModelName(_name), _size, bundle);
        }
    }

    public sealed class BundleConfiguration
    {
        private string _name;
        private readonly List<SingletonVariableModel> _singletons;
        private readonly WorkspaceModel _workspace;

        public BundleConfiguration(WorkspaceModel workspace)
        {
            Contract.Requires<ArgumentNullException>(workspace != null);
            _workspace = workspace;
            _singletons = new List<SingletonVariableModel>();
        }

        public BundleConfiguration WithName(string name)
        {
            _name = name;
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
            foreach (var aSingleton in _singletons)
            {
                newBundle.AddSingleton(aSingleton);
            }

            return newBundle;
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
            Contract.Requires<ArgumentNullException>(workspace != null);
            _workspace = workspace;
        }

        public void WithName(string variableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(variableName));
            _name = variableName;
        }

        public void WithSize(int variableSize)
        {
            Contract.Requires<ArgumentOutOfRangeException>(variableSize > 0);
            _size = variableSize;
        }

        public void WithDomain(string domainExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(domainExpression));
            _domainExpression = domainExpression;
        }

        public AggregateVariableModel Build()
        {
            return new AggregateVariableModel(_workspace, new ModelName(_name), _size, new InlineDomainModel(_domainExpression));
        }
    }
}