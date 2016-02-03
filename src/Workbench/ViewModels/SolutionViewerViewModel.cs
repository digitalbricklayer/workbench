using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution.
    /// </summary>
    public sealed class SolutionViewerViewModel : Conductor<GraphicViewModel>.Collection.AllActive
    {
        private IObservableCollection<ValueViewModel> values;
        private IObservableCollection<VariableVisualizerViewerViewModel> visualizers;

        /// <summary>
        /// Initialize the solution with a solution model.
        /// </summary>
        /// <param name="theSolution">The solution model.</param>
        public SolutionViewerViewModel(SolutionModel theSolution)
        {
            if (theSolution == null)
                throw new ArgumentNullException("theSolution");

            this.values = new BindableCollection<ValueViewModel>();
            this.visualizers = new BindableCollection<VariableVisualizerViewerViewModel>();
            this.Model = new SolutionModel();
        }

        /// <summary>
        /// Initialize the solution with default values.
        /// </summary>
        public SolutionViewerViewModel()
        {
            this.Values = new BindableCollection<ValueViewModel>();
            this.visualizers = new BindableCollection<VariableVisualizerViewerViewModel>();
            this.Model = new SolutionModel();
        }

        /// <summary>
        /// Gets the values displayed in the solution.
        /// </summary>
        public IObservableCollection<ValueViewModel> Values
        {
            get { return values; }
            set
            {
                this.values = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the variable visualizers.
        /// </summary>
        public IObservableCollection<VariableVisualizerViewerViewModel> Visualizers
        {
            get { return visualizers; }
            set
            {
                this.visualizers = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the solution model.
        /// </summary>
        public SolutionModel Model { get; set; }

        /// <summary>
        /// Bind the values to the solution.
        /// </summary>
        /// <param name="theValues">Values.</param>
        public void BindTo(IEnumerable<ValueViewModel> theValues)
        {
            this.Reset();
            foreach (var value in theValues)
                this.Values.Add(value);
        }

        /// <summary>
        /// Reset the contents of the solution.
        /// </summary>
        public void Reset()
        {
            this.Values.Clear();
        }

        /// <summary>
        /// Add a value.
        /// </summary>
        /// <param name="newValueViewModel">New value.</param>
        public void AddValue(ValueViewModel newValueViewModel)
        {
            if (newValueViewModel == null)
                throw new ArgumentNullException("newValueViewModel");
            this.Values.Add(newValueViewModel);
        }

        /// <summary>
        /// Add a new variable visualizer.
        /// </summary>
        /// <param name="newVariableVisualizer">New visualizer.</param>
        public void AddVisualzer(VariableVisualizerViewerViewModel newVariableVisualizer)
        {
            if (newVariableVisualizer == null)
                throw new ArgumentNullException("newVariableVisualizer");
            this.ActivateItem(newVariableVisualizer);
            this.Visualizers.Add(newVariableVisualizer);
        }
    }
}
