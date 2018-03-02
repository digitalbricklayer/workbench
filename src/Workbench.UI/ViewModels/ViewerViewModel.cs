using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public abstract class ViewerViewModel : GraphicViewModel
    {
        private GraphicModel model;

        protected ViewerViewModel(GraphicModel theGraphicModel)
            : base(theGraphicModel)
        {
            GraphicModel = theGraphicModel;
        }

        /// <summary>
        /// Gets or sets the viewer model.
        /// </summary>
        public GraphicModel GraphicModel
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
            get
            {
                if (Model is VisualizerModel visualizerModel) return visualizerModel.Title.Text;
                return string.Empty;
            }
            set
            {
                if (Model is VisualizerModel visualizerModel)
                {
                    visualizerModel.Title.Text = value;
                }
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Update the viewer prior to being displayed in the solution space.
        /// </summary>
        public virtual void Update()
        {
            // Default implementation...
        }
    }
}
