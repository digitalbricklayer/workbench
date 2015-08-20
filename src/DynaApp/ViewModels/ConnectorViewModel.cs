using System;
using System.Windows;
using DynaApp.Models;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// A connector (or connection point) can be attached to a 
    /// variable and is used to connect the variable to a domain.
    /// </summary>
    public class ConnectorViewModel : AbstractViewModel
    {
        /// <summary>
        /// The hotspot (or center) of the connector.
        /// This is pushed through from ConnectorItem in the UI.
        /// </summary>
        private Point hotspot;

        /// <summary>
        /// Initialize a connector view model with default values.
        /// </summary>
        public ConnectorViewModel()
        {
            this.Model = new ConnectorModel();
        }

        /// <summary>
        /// Gets or sets the connector identity.
        /// </summary>
        public int ConnectorIdentity { get; set; }

        /// <summary>
        /// The connection that is attached to this connector, or null if no connection is attached.
        /// </summary>
        public ConnectionViewModel AttachedConnection
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the graphic the connector is attached to, or null if the 
        /// connector is not attached to anything.
        /// </summary>
        public GraphicViewModel Parent
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets and sets the hotspot (or center) of the connector.
        /// This is pushed through from ConnectorItem in the UI.
        /// </summary>
        public Point Hotspot
        {
            get
            {
                return hotspot;
            }
            set
            {
                if (hotspot == value)
                {
                    return;
                }

                hotspot = value;
                OnHotspotUpdated();
            }
        }

        /// <summary>
        /// Gets or sets the connector model.
        /// </summary>
        public ConnectorModel Model { get; set; }

        /// <summary>
        /// Event raised when the connector hotspot has been updated.
        /// </summary>
        public event EventHandler<EventArgs> HotspotUpdated;

        /// <summary>
        /// Called when the connector hotspot has been updated.
        /// </summary>
        private void OnHotspotUpdated()
        {
            OnPropertyChanged("Hotspot");

            if (HotspotUpdated != null)
            {
                HotspotUpdated(this, EventArgs.Empty);
            }
        }
    }
}
