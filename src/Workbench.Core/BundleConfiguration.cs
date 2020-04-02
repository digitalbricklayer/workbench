using System;
using System.Collections.Generic;
using Workbench.Core.Models;

namespace Workbench.Core
{
    public sealed class BundleConfiguration
    {
        private string _name;
        private readonly List<SingletonVariableModel> _singletons;
        private readonly List<ConstraintModel> _constraints;
        private readonly WorkspaceModel _workspace;
        private readonly List<BucketVariableModel> _buckets;

        public BundleConfiguration(WorkspaceModel workspace)
        {
            _name = string.Empty;
            _workspace = workspace;
            _singletons = new List<SingletonVariableModel>();
            _buckets = new List<BucketVariableModel>();
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

        public BundleConfiguration AddBucket(Action<BucketConfiguration> action)
        {
            var bucketConfiguration = new BucketConfiguration(_workspace);

            action(bucketConfiguration);

            var newBucket = bucketConfiguration.Build();
            _buckets.Add(newBucket);

            return this;
        }

        public BundleModel Build()
        {
            var newBundle = new BundleModel(new ModelName(_name));
            AddSingletons(newBundle);
            AddBuckets(newBundle);
            AddConstraints(newBundle);

            return newBundle;
        }

        private void AddBuckets(BundleModel bundle)
        {
            foreach (var bucket in _buckets)
            {
                bundle.AddBucket(bucket);
            }
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
}
