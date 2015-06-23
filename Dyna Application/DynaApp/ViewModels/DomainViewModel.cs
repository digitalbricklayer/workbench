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
            this.Expression = string.Empty;
            this.PopulateConnectors();
        }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public string Expression { get; set; }

        private void PopulateConnectors()
        {
            this.Connectors.Add(new ConnectorViewModel());
            this.Connectors.Add(new ConnectorViewModel());
            this.Connectors.Add(new ConnectorViewModel());
            this.Connectors.Add(new ConnectorViewModel());
        }
    }
}
