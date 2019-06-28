using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Solvers;

namespace Workbench.Core.Models
{
    /// <summary>
    /// One solution to a model.
    /// </summary>
    [Serializable]
    public class SolutionModel : AbstractModel
    {
        private SolutionSnapshot snapshot;

        /// <summary>
        /// Initialize the solution with the model and snapshot.
        /// </summary>
        /// <param name="theModel">Model that the solution is supposed to solve.</param>
        /// <param name="theSnapshot">Solution snapshot.</param>
        /// <param name="duration">Time taken to find the solution.</param>
        public SolutionModel(ModelModel theModel, SolutionSnapshot theSnapshot, TimeSpan duration)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(theSnapshot != null);

            Model = theModel;
            Snapshot = theSnapshot;
            Snapshot = new SolutionSnapshot();
            Duration = duration;
        }

        /// <summary>
        /// Initialize the solution with the model.
        /// </summary>
        /// <param name="theModel">Model that the solution is supposed to solve.</param>
        public SolutionModel(ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);

            Model = theModel;
            Snapshot = new SolutionSnapshot();
            Snapshot = new SolutionSnapshot();
        }

        /// <summary>
        /// Gets or sets the solution snapshot.
        /// </summary>
        public SolutionSnapshot Snapshot
        {
            get { return this.snapshot; }
            set
            {
                this.snapshot = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the model this solution solves.
        /// </summary>
        public ModelModel Model { get; private set; }

        /// <summary>
        /// Gets the time taken to solve the model.
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Get the value matching the name.
        /// </summary>
        /// <param name="theVariableName">Text of the variable to find.</param>
        /// <returns>Value matching the name. Null if no value matches the name.</returns>
        public SingletonVariableLabelModel GetLabelByVariableName(string theVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVariableName));
            return Snapshot.GetSingletonLabelByVariableName(theVariableName);
        }

        /// <summary>
        /// Get the aggregate value matching the name.
        /// </summary>
        /// <param name="theVariableName">Aggregate value.</param>
        /// <returns>Aggregate value matching the name. Null if no aggregates matche the name.</returns>
        public AggregateVariableLabelModel GetCompoundLabelByVariableName(string theVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVariableName));
            return Snapshot.GetAggregateLabelByVariableName(theVariableName);
        }

        /// <summary>
        /// Update the solution from a snapshot.
        /// </summary>
        /// <param name="theSolveResult">Solution snapshot.</param>
        public void UpdateFrom(SolveResult theSolveResult)
        {
            Contract.Requires<ArgumentNullException>(theSolveResult != null);
            Snapshot = theSolveResult.Snapshot;
        }
    }
}
