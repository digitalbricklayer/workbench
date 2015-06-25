using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// A connection between two connectors.
    /// </summary>
    public sealed class ConnectionViewModel : AbstractViewModel
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
        /// Points that make up the connection.
        /// </summary>
        private PointCollection points;

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
                OnConnectionChanged();
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
                OnConnectionChanged();
            }
        }

        /// <summary>
        /// Gets or sets the source connector hotspot used for generating connection points.
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

                ComputeConnectionPoints();

                OnPropertyChanged("SourceConnectorHotspot");
            }
        }

        /// <summary>
        /// Gets or sets the destination connector hotspot used for generating connection points.
        /// </summary>
        public Point DestinationConnectorHotspot
        {
            get
            {
                return destinationConnectorHotspot;
            }
            set
            {
                destinationConnectorHotspot = value;

                ComputeConnectionPoints();

                OnPropertyChanged("DestinationConnectorHotspot");
            }
        }

        /// <summary>
        /// Gets whether the connection has been completed?
        /// </summary>
        public bool IsConnectionComplete
        {
            get
            {
                return this.DestinationConnector != null;
            }
        }

        /// <summary>
        /// Gets and sets the points that make up the connection.
        /// </summary>
        public PointCollection Points
        {
            get
            {
                return points;
            }
            set
            {
                points = value;

                OnPropertyChanged("Points");
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
        /// Event fired when the connection has changed.
        /// </summary>
        public event EventHandler<EventArgs> ConnectionChanged;

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

        /// <summary>
        /// Rebuild connection points.
        /// </summary>
        private void ComputeConnectionPoints()
        {
            PointCollection computedPoints = new PointCollection();
            computedPoints.Add(this.SourceConnectorHotspot);

            double deltaX = Math.Abs(this.DestinationConnectorHotspot.X - this.SourceConnectorHotspot.X);
            double deltaY = Math.Abs(this.DestinationConnectorHotspot.Y - this.SourceConnectorHotspot.Y);
            if (deltaX > deltaY)
            {
                double midPointX = this.SourceConnectorHotspot.X + ((this.DestinationConnectorHotspot.X - this.SourceConnectorHotspot.X) / 2);
                computedPoints.Add(new Point(midPointX, this.SourceConnectorHotspot.Y));
                computedPoints.Add(new Point(midPointX, this.DestinationConnectorHotspot.Y));
            }
            else
            {
                double midPointY = this.SourceConnectorHotspot.Y + ((this.DestinationConnectorHotspot.Y - this.SourceConnectorHotspot.Y) / 2);
                computedPoints.Add(new Point(this.SourceConnectorHotspot.X, midPointY));
                computedPoints.Add(new Point(this.DestinationConnectorHotspot.X, midPointY));
            }

            computedPoints.Add(this.DestinationConnectorHotspot);
            computedPoints.Freeze();

            this.Points = computedPoints;
        }

        /// <summary>
        /// Raises the 'ConnectionChanged' event.
        /// </summary>
        private void OnConnectionChanged()
        {
            if (ConnectionChanged != null)
            {
                ConnectionChanged(this, EventArgs.Empty);
            }
        }
    }
}
