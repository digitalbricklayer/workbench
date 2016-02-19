using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution designer.
    /// </summary>
    public sealed class SolutionDesignerViewModel : Conductor<VariableVisualizerDesignViewModel>.Collection.AllActive
    {
        private DisplayModel model;

        /// <summary>
        /// Initialize a solution designer view model with default values.
        /// </summary>
        public SolutionDesignerViewModel(DisplayModel theModel)
        {
            this.Model = theModel;
        }

        /// <summary>
        /// Gets the visualizer model.
        /// </summary>
        public DisplayModel Model
        {
            get { return this.model; }
            set
            {
                this.model = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the variable visualizers.
        /// </summary>
        public IObservableCollection<VariableVisualizerDesignViewModel> Visualizers
        {
            get { return this.Items; }
        }

        /// <summary>
        /// Add a variable visualizer.
        /// </summary>
        /// <param name="newVisualizer">New variable visualizer.</param>
        public void AddVisualizer(VariableVisualizerDesignViewModel newVisualizer)
        {
            this.ActivateItem(newVisualizer);
        }
    }
}
