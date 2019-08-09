using System;

namespace Workbench.Core.Models
{
    [Serializable]
    public abstract class VisualizerModel : GraphicModel
    {
        private WorkspaceTabTitle title;

        /// <summary>
        /// Initialize an unbound visualizer with a name and location.
        /// </summary>
        /// <param name="theModel"></param>
        /// <param name="theTitle"></param>
        protected VisualizerModel(Model theModel, WorkspaceTabTitle theTitle)
            : base(theModel)
        {
            Title = theTitle;
        }

        /// <summary>
        /// Gets or sets the visualizer title.
        /// </summary>
        public WorkspaceTabTitle Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                OnPropertyChanged();
            }
        }
    }
}
