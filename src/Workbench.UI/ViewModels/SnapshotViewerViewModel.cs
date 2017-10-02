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
        private IObservableCollection<ValueModel> values;

        public SnapshotViewerViewModel()
        {
            this.values = new BindableCollection<ValueModel>();
        }

        /// <summary>
        /// Gets the values displayed in the solution.
        /// </summary>
        public IObservableCollection<ValueModel> Values
        {
            get { return this.values; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.values = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Reset the contents of the solution.
        /// </summary>
        public void Reset()
        {
            Values.Clear();
        }

        /// <summary>
        /// Add a value.
        /// </summary>
        /// <param name="newValueViewModel">New value.</param>
        public void AddValue(ValueModel newValueViewModel)
        {
            Contract.Requires<ArgumentNullException>(newValueViewModel != null);
            Values.Add(newValueViewModel);
        }

        public void BindTo(SolutionModel theSolution)
        {
            Values.AddRange(theSolution.Snapshot.SingletonValues);
            Values.AddRange(theSolution.Snapshot.AggregateValues);
        }
    }
}