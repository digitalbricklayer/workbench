using System.Linq;
using System.Windows;
using DynaApp.Models;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for a variable.
    /// </summary>
    public sealed class VariableViewModel : GraphicViewModel
    {
        private VariableModel model;

        /// <summary>
        /// Initialize a variable with the new name.
        /// </summary>
        public VariableViewModel(string newName, Point newLocation)
            : base(newName, newLocation)
        {
            this.Model = new VariableModel();
        }

        /// <summary>
        /// Initialize a variable with the new name.
        /// </summary>
        public VariableViewModel(string newName)
            : base(newName)
        {
            this.Model = new VariableModel();
        }

        /// <summary>
        /// Initialize a variable with default values.
        /// </summary>
        public VariableViewModel()
            : this("New variable")
        {
        }

        /// <summary>
        /// Gets or sets the variable model.
        /// </summary>
        public new VariableModel Model
        {
            get { return this.model; }
            set
            {
                base.Model = value;
                this.model = value;
            }
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
    }
}
