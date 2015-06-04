using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for a variable.
    /// </summary>
    public sealed class VariableViewModel : AbstractModelBase
    {
        /// <summary>
        /// The X coordinate for the position of the variable.
        /// </summary>
        private double x;

        /// <summary>
        /// The Y coordinate for the position of the variable.
        /// </summary>
        private double y;

        /// <summary>
        /// Set to 'true' when the variable is selected.
        /// </summary>
        private bool isSelected = false;

        /// <summary>
        /// The variable name.
        /// </summary>
        private string name;

        /// <summary>
        /// Initialize a variable with the new name.
        /// </summary>
        public VariableViewModel(string newName)
        {
            this.Name = newName;
            this.Connectors = new ObservableCollection<ConnectorViewModel>();
            this.PopulateConnectors();
        }

        /// <summary>
        /// Initialize a variable with default values.
        /// </summary>
        public VariableViewModel()
        {
            this.Name = string.Empty;
        }

        /// <summary>
        /// Gets or sets the variable name.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                if (this.name == value) return;
                this.name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// The X coordinate for the position of the variable.
        /// </summary>
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                if (x == value) return;
                x = value;
                OnPropertyChanged("X");
            }
        }

        /// <summary>
        /// The Y coordinate for the position of the variable.
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                if (y == value) return;
                y = value;
                OnPropertyChanged("Y");
            }
        }

        /// <summary>
        /// Gets or sets selected status of the variable.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (isSelected == value) return;
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        /// <summary>
        /// Gets the connectors (connection anchor points) attached to the variable.
        /// </summary>
        public ObservableCollection<ConnectorViewModel> Connectors { get; private set; }

        /// <summary>
        /// Gets the list of all connections attached to the variable. 
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
