using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Castle.Core.Internal;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution viewer.
    /// </summary>
    public sealed class SolutionViewerViewModel : Conductor<VisualizerViewerViewModel>.Collection.AllActive
    {
        private IObservableCollection<ValueModel> values;

        /// <summary>
        /// Initialize the solution with a solution model.
        /// </summary>
        /// <param name="theSolution">The solution model.</param>
        public SolutionViewerViewModel(SolutionModel theSolution)
        {
            Contract.Requires<ArgumentNullException>(theSolution != null);

            this.values = new BindableCollection<ValueModel>();
            Model = theSolution;
        }

        /// <summary>
        /// Gets the values displayed in the solution.
        /// </summary>
        public IObservableCollection<ValueModel> Values
        {
            get { return values; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.values = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the solution model.
        /// </summary>
        public SolutionModel Model { get; private set; }

        /// <summary>
        /// Bind the solution model to the solution view model.
        /// </summary>
        /// <param name="theSolution">Solution model.</param>
        public void BindTo(SolutionModel theSolution)
        {
            Contract.Requires<ArgumentNullException>(theSolution != null);
            Items.ForEach(viewer => viewer.Update());
            Reset();
            Model = theSolution;
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

        /// <summary>
        /// Add a new variable visualizer.
        /// </summary>
        /// <param name="newVisualizer">New visualizer.</param>
        public void AddVisualizer(VisualizerViewerViewModel newVisualizer)
        {
            Contract.Requires<ArgumentNullException>(newVisualizer != null);
            ActivateItem(newVisualizer);
        }
    }
}
