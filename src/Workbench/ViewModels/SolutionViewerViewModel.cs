using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution viewer.
    /// </summary>
    public sealed class SolutionViewerViewModel : Conductor<VariableVisualizerViewerViewModel>.Collection.AllActive
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
            this.Model = theSolution;
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
        /// Bind the values to the solution.
        /// </summary>
        /// <param name="theValues">Variable values.</param>
        public void BindTo(IEnumerable<ValueModel> theValues)
        {
            Contract.Requires<ArgumentNullException>(theValues != null);
            this.Reset();
            foreach (var aValue in theValues)
            {
                var theVisualizer = this.GetVisualizerFor(aValue.VariableName);
                if (theVisualizer != null)
                    theVisualizer.Value = aValue;
                this.Values.Add(aValue);
            }
        }

        /// <summary>
        /// Reset the contents of the solution.
        /// </summary>
        public void Reset()
        {
            foreach (var aVisualizer in this.Items)
            {
                aVisualizer.Unbind();
            }
            this.Values.Clear();
        }

        /// <summary>
        /// Add a value.
        /// </summary>
        /// <param name="newValueViewModel">New value.</param>
        public void AddValue(ValueModel newValueViewModel)
        {
            Contract.Requires<ArgumentNullException>(newValueViewModel != null);
            this.Values.Add(newValueViewModel);
        }

        /// <summary>
        /// Add a new variable visualizer.
        /// </summary>
        /// <param name="newVariableVisualizer">New visualizer.</param>
        public void AddVisualizer(VariableVisualizerViewerViewModel newVariableVisualizer)
        {
            Contract.Requires<ArgumentNullException>(newVariableVisualizer != null);
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
        public VariableVisualizerViewerViewModel GetVisualizerFor(string variableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(variableName));
            return (from x in this.Items
                    where x.Binding.Name == variableName
                    select x).FirstOrDefault();
        }
    }
}
