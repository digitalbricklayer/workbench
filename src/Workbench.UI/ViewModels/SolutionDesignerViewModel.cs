using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution designer.
    /// </summary>
    public sealed class SolutionDesignerViewModel : Conductor<VisualizerDesignerViewModel>.Collection.AllActive
    {
        private DisplayModel model;

        /// <summary>
        /// Initialize a solution designer view model with default values.
        /// </summary>
        public SolutionDesignerViewModel(DisplayModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Model = theModel;
        }

        /// <summary>
        /// Gets the visualizer model.
        /// </summary>
        public DisplayModel Model
        {
            get { return this.model; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.model = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the variable visualizers.
        /// </summary>
        public IObservableCollection<VisualizerDesignerViewModel> Visualizers
        {
            get
            {
                return this.Items;
            }
        }

        /// <summary>
        /// Add a variable visualizer.
        /// </summary>
        /// <param name="newVisualizer">New variable visualizer.</param>
        public void AddVisualizer(VisualizerDesignerViewModel newVisualizer)
        {
            Contract.Requires<ArgumentNullException>(newVisualizer != null);
            Model.AddVisualizer(newVisualizer.Model);
            FixupVisualizer(newVisualizer);
        }

        /// <summary>
        /// Fixes up a visualizer view model into the design view model.
        /// </summary>
        /// <remarks>
        /// Used when mapping the model to a view model.
        /// </remarks>
        /// <param name="newVisualizerViewModel">Visualizers design view model.</param>
        internal void FixupVisualizer(VisualizerDesignerViewModel newVisualizerViewModel)
        {
            Contract.Requires<ArgumentNullException>(newVisualizerViewModel != null);
            ActivateItem(newVisualizerViewModel);
        }
    }
}
