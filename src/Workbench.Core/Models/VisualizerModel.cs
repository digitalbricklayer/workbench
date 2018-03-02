using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core.Models
{
    [Serializable]
    public abstract class VisualizerModel : GraphicModel
    {
        private VisualizerTitle title;

        /// <summary>
        /// Initialize an unbound visualizer with a name and location.
        /// </summary>
        /// <param name="theModel"></param>
        /// <param name="location">Location.</param>
        protected VisualizerModel(Model theModel, VisualizerTitle theTitle, Point location)
            : base(theModel, location)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(theTitle != null);
            Title = theTitle;
        }

        /// <summary>
        /// Gets or sets the visualizer title.
        /// </summary>
        public VisualizerTitle Title
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
