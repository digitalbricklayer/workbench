using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
        /// Initialize a solution snapshot with singleton labels and aggregate labels.
        /// </summary>
        public SolutionSnapshot(IEnumerable<SingletonVariableLabelModel> singletonVariableLabels, IEnumerable<AggregateVariableLabelModel> aggregateVariableLabel)
        {
            _singletonVariableLabels = new List<SingletonVariableLabelModel>(singletonVariableLabels);
            _aggregateVariableLabels = new List<AggregateVariableLabelModel>(aggregateVariableLabel);
            _bucketLabels = new List<BucketLabelModel>();
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
                Contract.Ensures(Contract.Result<IReadOnlyCollection<SingletonVariableLabelModel>>() != null);
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
                Contract.Ensures(Contract.Result<IReadOnlyCollection<AggregateVariableLabelModel>>() != null);
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
        /// Add a label to the snapshot.
        /// </summary>
        /// <param name="newSingletonVariableLabel">Singleton label.</param>
        internal void AddSingletonLabel(SingletonVariableLabelModel newSingletonVariableLabel)
        {
            Contract.Requires<ArgumentNullException>(newSingletonVariableLabel != null);
            _singletonVariableLabels.Add(newSingletonVariableLabel);
        }

        /// <summary>
        /// Add a compound label to the snapshot.
        /// </summary>
        /// <param name="newAggregateVariableLabel">Aggregate label.</param>
        internal void AddAggregateLabel(AggregateVariableLabelModel newAggregateVariableLabel)
        {
            Contract.Requires<ArgumentNullException>(newAggregateVariableLabel != null);
            _aggregateVariableLabels.Add(newAggregateVariableLabel);
        }

        /// <summary>
        /// Add bucket label.
        /// </summary>
        /// <param name="bucketLabel">Bucket label.</param>
        internal void AddBucketLabel(BucketLabelModel bucketLabel)
        {
            Contract.Requires<ArgumentNullException>(bucketLabel != null);
            _bucketLabels.Add(bucketLabel);
        }

        /// <summary>
        /// Get the compound label matching the variable name.
        /// </summary>
        /// <param name="theAggregateVariableName">Aggregate variable name.</param>
        /// <returns>Compound label for the aggregate variable.</returns>
        public AggregateVariableLabelModel GetCompoundLabelByVariableName(string theAggregateVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theAggregateVariableName));
            return _aggregateVariableLabels.FirstOrDefault(_ => _.Variable.Name.IsEqualTo(theAggregateVariableName));
        }

        /// <summary>
        /// Get the label matching the variable name.
        /// </summary>
        /// <param name="theSingletonVariableName">Singleton variable name.</param>
        /// <returns>Label for the singleton variable.</returns>
        public SingletonVariableLabelModel GetLabelByVariableName(string theSingletonVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theSingletonVariableName));
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