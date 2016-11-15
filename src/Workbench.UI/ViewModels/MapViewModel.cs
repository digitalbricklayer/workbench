using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class MapViewModel : Screen
    {
        public const string DefaultBackgroundImagePath = "pack://application:,,,/ConstraintWorkbench;component/Images/Transparent.png";
        private readonly MapModel model;

        public MapViewModel(MapModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            this.model = theModel;
            if (!this.model.HasBackgroundImage)
            {
                // Load a default background
                this.model.BackgroundImagePath = DefaultBackgroundImagePath;
            }
        }

        /// <summary>
        /// Gets the map model.
        /// </summary>
        public MapModel Map
        {
            get { return this.model; }
        }
    }
}
