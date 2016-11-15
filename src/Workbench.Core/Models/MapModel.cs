using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Drawing;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A map graphical model.
    /// </summary>
    [Serializable]
    public class MapModel : AbstractModel
    {
        private ObservableCollection<MapRegionModel> regions;
        private string backgroundImagePath;

        /// <summary>
        /// Initialize the map with an initial set of regions.
        /// </summary>
        /// <param name="theRegions">Initial set of regions.</param>
        public MapModel(IEnumerable<MapRegionModel> theRegions)
        {
            Contract.Requires<ArgumentNullException>(theRegions != null);
            this.regions = new ObservableCollection<MapRegionModel>(theRegions);
        }

        /// <summary>
        /// Initialize the map with an initial set of regions.
        /// </summary>
        /// <param name="theRegions">Initial set of regions.</param>
        public MapModel(params MapRegionModel[] theRegions)
        {
            Contract.Requires<ArgumentNullException>(theRegions != null);
            this.regions = new ObservableCollection<MapRegionModel>(theRegions);
        }

        /// <summary>
        /// Initalize a map without regions.
        /// </summary>
        public MapModel()
        {
            this.regions = new ObservableCollection<MapRegionModel>();
        }

        /// <summary>
        /// Gets or sets the map regions.
        /// </summary>
        public ObservableCollection<MapRegionModel> Regions
        {
            get { return this.regions; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.regions = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the map background image path.
        /// </summary>
        public string BackgroundImagePath
        {
            get { return this.backgroundImagePath; }
            set
            {
                this.backgroundImagePath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets whether there is a background image.
        /// </summary>
        public bool HasBackgroundImage
        {
            get { return !string.IsNullOrWhiteSpace(this.backgroundImagePath); }
        }

        /// <summary>
        /// Add a region to the map.
        /// </summary>
        /// <param name="theRegion">The new region.</param>
        public void AddRegion(MapRegionModel theRegion)
        {
            Contract.Requires<ArgumentNullException>(theRegion != null);
            Regions.Add(theRegion);
        }
    }
}
