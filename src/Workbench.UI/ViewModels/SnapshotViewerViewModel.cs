using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the built-in snapshot viewer.
    /// </summary>
    public class SnapshotViewerViewModel : Screen
    {
        private IObservableCollection<LabelModel> labels;
        private IObservableCollection<CompoundLabelModel> compoundLabels;

        public SnapshotViewerViewModel()
        {
            this.labels = new BindableCollection<LabelModel>();
        }

        /// <summary>
        /// Gets the labels displayed in the solution.
        /// </summary>
        public IObservableCollection<LabelModel> Labels
        {
            get { return this.labels; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.labels = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the compound labels displayed in the solution.
        /// </summary>
        public IObservableCollection<CompoundLabelModel> CompoundLabels
        {
            get { return this.compoundLabels; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.compoundLabels = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Reset the contents of the solution.
        /// </summary>
        public void Reset()
        {
            Labels.Clear();
            CompoundLabels.Clear();
        }

        /// <summary>
        /// Add a value.
        /// </summary>
        /// <param name="newLabelViewModel">New value.</param>
        public void AddValue(LabelModel newLabelViewModel)
        {
            Contract.Requires<ArgumentNullException>(newLabelViewModel != null);
            Labels.Add(newLabelViewModel);
        }

        public void BindTo(SolutionModel theSolution)
        {
            Labels.AddRange(theSolution.Snapshot.SingletonValues);
            CompoundLabels.AddRange(theSolution.Snapshot.AggregateValues);
        }
    }
}