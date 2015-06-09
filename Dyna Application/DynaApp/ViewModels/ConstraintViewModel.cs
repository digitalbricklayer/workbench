using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for a constraint.
    /// </summary>
    public class ConstraintViewModel : GraphicViewModel
    {
        public ConstraintViewModel(string newConstraintName)
            : base(newConstraintName)
        {
            this.Connectors = new ObservableCollection<ConnectorViewModel>();
            this.PopulateConnectors();
        }

        /// <summary>
        /// Gets the connectors (connection anchor points) attached to the domain.
        /// </summary>
        public ObservableCollection<ConnectorViewModel> Connectors { get; private set; }

        /// <summary>
        /// Gets the list of all connections attached to the domain. 
        /// </summary>
        public ICollection<ConnectionViewModel> AttachedConnections
        {
            get
            {
                List<ConnectionViewModel> attachedConnections = new List<ConnectionViewModel>();

                foreach (var connector in this.Connectors)
                {
                    if (connector.AttachedConnection != null)
                    {
                        attachedConnections.Add(connector.AttachedConnection);
                    }
                }

                return attachedConnections;
            }
        }

        private void PopulateConnectors()
        {
            this.Connectors.Add(new ConnectorViewModel());
            this.Connectors.Add(new ConnectorViewModel());
            this.Connectors.Add(new ConnectorViewModel());
            this.Connectors.Add(new ConnectorViewModel());
        }
    }
}
