using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the variable visualizer in viewer mode.
    /// </summary>
    public sealed class VariableVisualizerViewerViewModel : GraphicViewModel
    {
        private VariableViewModel boundTo;
        private VariableVisualizerModel model;
        private ValueViewModel value;

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
        /// Gets or sets the variable binding.
        /// </summary>
        public VariableViewModel Binding
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

        /// <summary>
        /// Gets or sets the variable value;
        /// </summary>
        public ValueViewModel Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Unbind the value from viewer.
        /// </summary>
        public void Unbind()
        {
            this.Value = null;
        }
    }
}
