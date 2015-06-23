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
            this.Expression = string.Empty;
            this.PopulateConnectors();
        }

        /// <summary>
        /// Gets or sets the constraint expression.
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
