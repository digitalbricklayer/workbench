using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution viewer.
    /// </summary>
    public sealed class SolutionViewerViewModel : Conductor<VariableVisualizerViewerViewModel>.Collection.AllActive
    {
        private IObservableCollection<ValueViewModel> values;

        /// <summary>
        /// Initialize the solution with a solution model.
        /// </summary>
        /// <param name="theSolution">The solution model.</param>
        public SolutionViewerViewModel(SolutionModel theSolution)
        {
            if (theSolution == null)
                throw new ArgumentNullException("theSolution");

            this.values = new BindableCollection<ValueViewModel>();
            this.Model = theSolution;
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
        /// Gets or sets the solution model.
        /// </summary>
        public SolutionModel Model { get; set; }

        /// <summary>
        /// Bind the values to the solution.
        /// </summary>
        /// <param name="theValues">SingletonValues.</param>
        public void BindTo(IEnumerable<ValueViewModel> theValues)
        {
            this.Reset();
            foreach (var value in theValues)
            {
                this.Values.Add(value);
            }
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
        public void AddVisualizer(VariableVisualizerViewerViewModel newVariableVisualizer)
        {
            if (newVariableVisualizer == null)
                throw new ArgumentNullException("newVariableVisualizer");
            this.ActivateItem(newVariableVisualizer);
        }

        /// <summary>
        /// Unbind all viewers from their existing values.
        /// </summary>
        public void UnbindAll()
        {
            foreach (var aViewer in this.Items)
            {
                aViewer.Unbind();
            }
        }

        /// <summary>
        /// Get the visualizer bound to the variable matching the variable name.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <returns>Visualizer bound to the variable matching the variable name.</returns>
        public VariableVisualizerModel GetVisualizerFor(string variableName)
        {
            return this.Model.GetVisualizerFor(variableName);
        }
    }
}
