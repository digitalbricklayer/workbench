using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the variable visualizer in design mode.
    /// </summary>
    public sealed class VariableVisualizerViewerViewModel : GraphicViewModel
    {
        private VariableViewModel boundTo;
        private VariableVisualizerModel model;

        /// <summary>
        /// Initialize the variable visualizer viewer view model with the variable visualizer model.
        /// </summary>
        /// <param name="theVariableVisualizerModel">Visualizer model.</param>
        public VariableVisualizerViewerViewModel(VariableVisualizerModel theVariableVisualizerModel)
            : base(theVariableVisualizerModel)
        {
            this.Model = theVariableVisualizerModel;
        }

        /// <summary>
        /// Gets or sets the variable model.
        /// </summary>
        public new VariableVisualizerModel Model
        {
            get { return this.model; }
            set
            {
                base.Model = value;
                this.model = value;
            }
        }

        /// <summary>
        /// Gets or sets the variable the visualizer is bound to.
        /// </summary>
        public VariableViewModel BoundTo
        {
            get
            {
                return this.boundTo;
            }
            set
            {
                this.boundTo = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
