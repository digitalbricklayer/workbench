using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Variable visualiser view model.
    /// </summary>
    public sealed class VariableVisualiserViewModel : GraphicViewModel
    {
        private VariableViewModel boundTo;

        /// <summary>
        /// Initialize the variable visualiser view model with the variable visualiser model.
        /// </summary>
        /// <param name="theVariableVisualiserModel"></param>
        public VariableVisualiserViewModel(VariableVisualiserModel theVariableVisualiserModel)
            : base(theVariableVisualiserModel)
        {
        }

        /// <summary>
        /// Gets or sets the variable the visualiser is bound to.
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
