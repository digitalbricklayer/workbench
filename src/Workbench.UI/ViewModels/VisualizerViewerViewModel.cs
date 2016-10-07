using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public abstract class VisualizerViewerViewModel : GraphicViewModel
    {
        private VariableViewModel boundTo;
        private ValueModel value;

        protected VisualizerViewerViewModel(GraphicModel theGraphicModel)
            : base(theGraphicModel)
        {
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
                Contract.Requires<ArgumentNullException>(value != null);
                this.boundTo = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the variable value;
        /// </summary>
        public ValueModel Value
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
