using System;
using System.Diagnostics;
using System.Windows;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// A connection between two connectors.
    /// </summary>
    public sealed class ConnectionViewModel : AbstractModelBase
    {
        /// <summary>
        /// The source connector the connection is attached to.
        /// </summary>
        private ConnectorViewModel sourceConnector;

        /// <summary>
        /// The destination connector the connection is attached to.
        /// </summary>
        private ConnectorViewModel destinationConnector;

        /// <summary>
        /// The source and dest hotspots used for generating connection points.
        /// </summary>
        private Point sourceConnectorHotspot;
        private Point destinationConnectorHotspot;

        /// <summary>
        /// Gets and sets the source connector.
        /// </summary>
        public ConnectorViewModel SourceConnector
        {
            get
            {
                return sourceConnector;
            }
            set
            {
                if (sourceConnector == value)
                {
                    return;
                }

                if (sourceConnector != null)
                {
                    Trace.Assert(sourceConnector.AttachedConnection == this);

                    sourceConnector.AttachedConnection = null;
                    sourceConnector.HotspotUpdated -= new EventHandler<EventArgs>(sourceConnector_HotspotUpdated);
                }

                sourceConnector = value;

                if (sourceConnector != null)
                {
                    Trace.Assert(sourceConnector.AttachedConnection == null);

                    sourceConnector.AttachedConnection = this;
                    sourceConnector.HotspotUpdated += new EventHandler<EventArgs>(sourceConnector_HotspotUpdated);
                    this.SourceConnectorHotspot = sourceConnector.Hotspot;
                }

                OnPropertyChanged("SourceConnector");
            }
        }

        /// <summary>
        /// Gets and sets the destination connector.
        /// </summary>
        public ConnectorViewModel DestinationConnector
        {
            get
            {
                return destinationConnector;
            }
            set
            {
                if (destinationConnector == value)
                {
                    return;
                }

                if (destinationConnector != null)
                {
                    Trace.Assert(destinationConnector.AttachedConnection == this);

                    destinationConnector.AttachedConnection = null;
                    destinationConnector.HotspotUpdated += new EventHandler<EventArgs>(destinationConnector_HotspotUpdated);
                }

                destinationConnector = value;

                if (destinationConnector != null)
                {
                    Trace.Assert(destinationConnector.AttachedConnection == null);

                    destinationConnector.AttachedConnection = this;
                    destinationConnector.HotspotUpdated += new EventHandler<EventArgs>(destinationConnector_HotspotUpdated);
                    this.DestinationConnectorHotspot = destinationConnector.Hotspot;
                }

                OnPropertyChanged("DestinationConnector");
            }
        }

        /// <summary>
        /// The source and dest hotspots used for generating connection points.
        /// </summary>
        public Point SourceConnectorHotspot
        {
            get
            {
                return sourceConnectorHotspot;
            }
            set
            {
                sourceConnectorHotspot = value;

                OnPropertyChanged("SourceConnectorHotspot");
            }
        }

        public Point DestinationConnectorHotspot
        {
            get
            {
                return destinationConnectorHotspot;
            }
            set
            {
                destinationConnectorHotspot = value;

                OnPropertyChanged("destinationConnectorHotspot");
            }
        }

        /// <summary>
        /// Initiate a connection with a source connector.
        /// </summary>
        /// <param name="theDraggedOutConnector">The source connector.</param>
        public void InitiateConnection(ConnectorViewModel theDraggedOutConnector)
        {
            if (theDraggedOutConnector == null) 
                throw new ArgumentNullException("theDraggedOutConnector");
            this.SourceConnector = theDraggedOutConnector;
        }

        /// <summary>
        /// Complete a connection between two connectors.
        /// </summary>
        /// <param name="theConnectorDraggedOver">The end connector.</param>
        public void CompleteConnection(ConnectorViewModel theConnectorDraggedOver)
        {
            if (theConnectorDraggedOver == null) 
                throw new ArgumentNullException("theConnectorDraggedOver");

            if (this.DestinationConnector == null)
            {
                this.DestinationConnector = theConnectorDraggedOver;
            }
            else
            {
                this.SourceConnector = theConnectorDraggedOver;
            }
        }

        /// <summary>
        /// Event raised when the hotspot of the source connector has been updated.
        /// </summary>
        private void sourceConnector_HotspotUpdated(object sender, EventArgs e)
        {
            this.SourceConnectorHotspot = this.SourceConnector.Hotspot;
        }

        /// <summary>
        /// Event raised when the hotspot of the dest connector has been updated.
        /// </summary>
        private void destinationConnector_HotspotUpdated(object sender, EventArgs e)
        {
            this.DestinationConnectorHotspot = this.DestinationConnector.Hotspot;
        }
    }
}
