using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public abstract class VisualizerViewerViewModel : GraphicViewModel
    {
        private VisualizerModel model;

        protected VisualizerViewerViewModel(GraphicModel theGraphicModel)
            : base(theGraphicModel)
        {
        }

        /// <summary>
        /// Gets or sets the designer model.
        /// </summary>
        public new VisualizerModel Model
        {
            get { return this.model; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);

                base.Model = value;
                this.model = value;
            }
        }

        /// <summary>
        /// Gets or sets the graphic title.
        /// </summary>
        public virtual string Title
        {
            get { return Model.Title; }
            set
            {
                Model.Title = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
