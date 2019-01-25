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
        private BundleModel _currentBundle;

        /// <summary>
        /// Initialize a workspace builder with a model name.
        /// </summary>
        public WorkspaceBuilder(string theModelName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theModelName));
            _workspace = new WorkspaceModel(new ModelName(theModelName));
            _currentBundle = _workspace.Model;
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
        public WorkspaceBuilder AddAggregate(Action<IAggregateVariableConfiguration> action)
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
            var theConstraintModel = new ExpressionConstraintModel(_workspace.Model, new ConstraintExpressionModel(theConstraintExpression));
            _workspace.Model.AddConstraint(theConstraintModel);
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

            var theChessboard = new ChessboardModel(new ModelName(theVisualizerName));
            var theChessboardVisualizer = new ChessboardTabModel(theChessboard, new WorkspaceTabTitle());
            _workspace.AddVisualizer(theChessboardVisualizer);
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

        public WorkspaceBuilder AddBundle(string bundleName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(bundleName));
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);

            var bundle = new BundleModel(new ModelName(bundleName));
            _currentBundle.AddBundle(bundle);
            _currentBundle = bundle;

            return this;
        }

        public WorkspaceBuilder AddBucket(string name, int size, string bucketName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(name));
            Contract.Requires<ArgumentOutOfRangeException>(size > 0);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(bucketName));
            Contract.Ensures(Contract.Result<WorkspaceBuilder>() != null);

            var newBucket = new BucketModel(new ModelName(name), size, bucketName);
            _currentBundle.AddBucket(newBucket);

            return this;
        }

        public WorkspaceModel Build()
        {
            Contract.Ensures(Contract.Result<WorkspaceModel>() != null);
            return _workspace;
        }

        private IAggregateVariableConfiguration CreateDefaultAggregateVariableConfig()
        {
            return new AggregateVariableConfiguration(_workspace);
        }
    }

    internal sealed class AggregateVariableConfiguration : IAggregateVariableConfiguration
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

    public interface IAggregateVariableConfiguration
    {
        void WithName(string variableName);
        void WithSize(int variableSize);
        void WithDomain(string domainExpression);
        AggregateVariableModel Build();
    }
}