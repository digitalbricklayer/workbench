using System.Linq;
using System.Windows;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for a variable.
    /// </summary>
    public sealed class VariableViewModel : GraphicViewModel
    {
        /// <summary>
        /// Initialize a variable with the new name.
        /// </summary>
        public VariableViewModel(string newName, Point newLocation)
            : base(newName, newLocation)
        {
            this.PopulateConnectors();
        }

        /// <summary>
        /// Initialize a variable with the new name.
        /// </summary>
        public VariableViewModel(string newName)
            : base(newName)
        {
            this.PopulateConnectors();
        }

        /// <summary>
        /// Initialize a variable with default values.
        /// </summary>
        public VariableViewModel()
            : this("New variable")
        {
        }

        /// <summary>
        /// Is the destination graphic connectable to the variable?
        /// </summary>
        /// <param name="destinationGraphic">Destination being connected to.</param>
        /// <returns>True if the destination can be connected, False if it cannot be connected.</returns>
        public override bool IsConnectableTo(GraphicViewModel destinationGraphic)
        {
            // Variables cannot connect to other variables...
            var destinationAsVariable = destinationGraphic as VariableViewModel;
            if (destinationAsVariable != null) return false;

            // Variables are not permitted to have two connections to the same destination...
            return this.AttachedConnections.Where(connection => connection.IsConnectionComplete)
                                           .All(connection => connection.DestinationConnector.Parent != destinationGraphic);
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
