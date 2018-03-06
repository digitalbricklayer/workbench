using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Workbench.Core.Models
{
    /// <summary>
    /// One solution to the model.
    /// </summary>
    [Serializable]
    public sealed class SolutionSnapshot
    {
        private readonly List<LabelModel> singletonLabels;
        private readonly List<CompoundLabelModel> compoundLabels;
        private TimeSpan duration;

        /// <summary>
        /// Initialize a solution snapshot with singleton labels, compound labels and the solution duration.
        /// </summary>
        public SolutionSnapshot(IEnumerable<LabelModel> theSingletonValues, IEnumerable<CompoundLabelModel> theCompoundLabels, TimeSpan theDuration)
        {
            this.singletonLabels = new List<LabelModel>(theSingletonValues);
            this.compoundLabels = new List<CompoundLabelModel>(theCompoundLabels);
	    this.duration = theDuration;
        }

        /// <summary>
        /// Initialize a solution snapshot with default values.
        /// </summary>
        public SolutionSnapshot()
        {
            this.singletonLabels = new List<LabelModel>();
            this.compoundLabels = new List<CompoundLabelModel>();
        }

        /// <summary>
        /// Gets the singleton variable values.
        /// </summary>
        public IReadOnlyCollection<LabelModel> SingletonValues
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<LabelModel>>() != null);
                return this.singletonLabels.ToList();
            }
        }

        /// <summary>
        /// Gets the aggregate variable values.
        /// </summary>
        public IReadOnlyCollection<CompoundLabelModel> AggregateValues
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<CompoundLabelModel>>() != null);
                return this.compoundLabels.ToList();
            }
        }

        /// <summary>
        /// Gets the time taken to solve the model.
        /// </summary>
        public TimeSpan Duration
        {
            get { return this.duration; }
            internal set { this.duration = value; }
        }

        /// <summary>
        /// Add a label to the snapshot.
        /// </summary>
        /// <param name="newLabel">Singleton value.</param>
        internal void AddSingletonValue(LabelModel newLabel)
        {
            Contract.Requires<ArgumentNullException>(newLabel != null);
            this.singletonLabels.Add(newLabel);
        }

        /// <summary>
        /// Add a compound label to the snapshot.
        /// </summary>
        /// <param name="newCompoundLabel">Aggregate value.</param>
        internal void AddAggregateValue(CompoundLabelModel newCompoundLabel)
        {
            Contract.Requires<ArgumentNullException>(newCompoundLabel != null);
            this.compoundLabels.Add(newCompoundLabel);
        }

        /// <summary>
        /// Get the compound label matching the variable name.
        /// </summary>
        /// <param name="theAggregateVariableName">Aggregate variable name.</param>
        /// <returns>Compound label for the aggregate variable.</returns>
        public CompoundLabelModel GetCompoundLabelByVariableName(string theAggregateVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theAggregateVariableName));
            return this.compoundLabels.FirstOrDefault(_ => _.Variable.Name == theAggregateVariableName);
        }

        /// <summary>
        /// Get the label matching the variable name.
        /// </summary>
        /// <param name="theSingletonVariableName">Singleton variable name.</param>
        /// <returns>Label for the singleton variable.</returns>
        public LabelModel GetLabelByVariableName(string theSingletonVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theSingletonVariableName));
            return this.singletonLabels.FirstOrDefault(_ => _.VariableName == theSingletonVariableName);
        }
    }
}