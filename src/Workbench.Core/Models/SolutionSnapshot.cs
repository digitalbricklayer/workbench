using System;
using System.Collections.Generic;
using System.Linq;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Represents a single solution to the model.
    /// </summary>
    [Serializable]
    public sealed class SolutionSnapshot
    {
        private readonly List<SingletonVariableLabelModel> _singletonVariableLabels;
        private readonly List<AggregateVariableLabelModel> _aggregateVariableLabels;
        private readonly List<BucketLabelModel> _bucketLabels;

        /// <summary>
        /// Initialize a solution snapshot with singleton labels, aggregate labels and bucket labels.
        /// </summary>
        public SolutionSnapshot(IEnumerable<SingletonVariableLabelModel> singletonVariableLabels,
                                IEnumerable<AggregateVariableLabelModel> aggregateVariableLabel,
                                IEnumerable<BucketLabelModel> bucketVariableLabels)
        {
            _singletonVariableLabels = new List<SingletonVariableLabelModel>(singletonVariableLabels);
            _aggregateVariableLabels = new List<AggregateVariableLabelModel>(aggregateVariableLabel);
            _bucketLabels = new List<BucketLabelModel>(bucketVariableLabels);
        }

        /// <summary>
        /// Initialize a solution snapshot with default values.
        /// </summary>
        public SolutionSnapshot()
        {
            _singletonVariableLabels = new List<SingletonVariableLabelModel>();
            _aggregateVariableLabels = new List<AggregateVariableLabelModel>();
            _bucketLabels = new List<BucketLabelModel>();
        }

        /// <summary>
        /// Gets the singleton variable labels.
        /// </summary>
        public IReadOnlyCollection<SingletonVariableLabelModel> SingletonLabels
        {
            get
            {
                return _singletonVariableLabels.ToList();
            }
        }

        /// <summary>
        /// Gets the aggregate variable labels.
        /// </summary>
        public IReadOnlyCollection<AggregateVariableLabelModel> AggregateLabels
        {
            get
            {
                return _aggregateVariableLabels.ToList();
            }
        }

        /// <summary>
        /// Gets the bucket labels.
        /// </summary>
        public IReadOnlyCollection<BucketLabelModel> BucketLabels => _bucketLabels.ToList();

        /// <summary>
        /// Gets an empty snapshot.
        /// </summary>
        public static SolutionSnapshot Empty => new SolutionSnapshot();

        /// <summary>
        /// Add a singleton label to the snapshot.
        /// </summary>
        /// <param name="newSingletonVariableLabel">Singleton label.</param>
        internal void AddSingletonLabel(SingletonVariableLabelModel newSingletonVariableLabel)
        {
            _singletonVariableLabels.Add(newSingletonVariableLabel);
        }

        /// <summary>
        /// Add an aggregate label to the snapshot.
        /// </summary>
        /// <param name="newAggregateVariableLabel">Aggregate label.</param>
        internal void AddAggregateLabel(AggregateVariableLabelModel newAggregateVariableLabel)
        {
            _aggregateVariableLabels.Add(newAggregateVariableLabel);
        }

        /// <summary>
        /// Add a bucket label to the snapshot.
        /// </summary>
        /// <param name="bucketLabel">Bucket label.</param>
        internal void AddBucketLabel(BucketLabelModel bucketLabel)
        {
            _bucketLabels.Add(bucketLabel);
        }

        /// <summary>
        /// Get the aggregate label matching the variable name.
        /// </summary>
        /// <param name="theAggregateVariableName">Aggregate variable name.</param>
        /// <returns>Aggregate label for the aggregate variable.</returns>
        public AggregateVariableLabelModel GetAggregateLabelByVariableName(string theAggregateVariableName)
        {
            if (string.IsNullOrWhiteSpace(theAggregateVariableName))
                throw new ArgumentException(nameof(theAggregateVariableName));

            return _aggregateVariableLabels.FirstOrDefault(_ => _.Variable.Name.IsEqualTo(theAggregateVariableName));
        }

        /// <summary>
        /// Get the singleton label matching the variable name.
        /// </summary>
        /// <param name="theSingletonVariableName">Singleton variable name.</param>
        /// <returns>Singleton label for the singleton variable.</returns>
        public SingletonVariableLabelModel GetSingletonLabelByVariableName(string theSingletonVariableName)
        {
            if (string.IsNullOrWhiteSpace(theSingletonVariableName))
                throw new ArgumentException(nameof(theSingletonVariableName));

            return _singletonVariableLabels.FirstOrDefault(_ => _.VariableName == theSingletonVariableName);
        }

        /// <summary>
        /// Get the bucket label matching the bucket name.
        /// </summary>
        /// <param name="bucketName">Bucket name.</param>
        /// <returns>Label for the named bucket.</returns>
        public BucketLabelModel GetBucketLabelByName(string bucketName)
        {
            return _bucketLabels.FirstOrDefault(bucketLabel => bucketLabel.Bucket.Name == bucketName);
        }
    }
}