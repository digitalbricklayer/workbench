using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution designer.
    /// </summary>
    public sealed class SolutionDesignerViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private IObservableCollection<VariableVisualizerViewModel> visualizers;
        private DisplayModel model;

        /// <summary>
        /// Initialize a solution designer view model with default values.
        /// </summary>
        public SolutionDesignerViewModel(DisplayModel theModel)
        {
            this.Visualizers = new BindableCollection<VariableVisualizerViewModel>();
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
        public IObservableCollection<VariableVisualizerViewModel> Visualizers
        {
            get { return this.visualizers; }
            set
            {
                this.visualizers = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Add a variable visualizer.
        /// </summary>
        /// <param name="newVisualizer">New variable visualizer.</param>
        public void AddVisualizer(VariableVisualizerViewModel newVisualizer)
        {
            this.ActivateItem(newVisualizer);
            this.Visualizers.Add(newVisualizer);
        }
    }
}
