using System.Collections.Generic;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for a domain.
    /// </summary>
    public sealed class DomainViewModel : GraphicViewModel
    {
        /// <summary>
        /// Initialize a variable with a new name.
        /// </summary>
        public DomainViewModel(string newDomainName)
            : base(newDomainName)
        {
            this.PopulateConnectors();
        }

        /// <summary>
        /// Is the destination graphic connectable to the graphic?
        /// </summary>
        /// <param name="destinationGraphic">Destination being connected to.</param>
        /// <returns>True if the destination can be connected, False if it cannot be connected.</returns>
        public override bool IsConnectableTo(GraphicViewModel destinationGraphic)
        {
            return false;
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
