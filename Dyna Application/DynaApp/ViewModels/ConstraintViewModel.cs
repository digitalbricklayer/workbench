namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for a constraint.
    /// </summary>
    public class ConstraintViewModel : GraphicViewModel
    {
        public ConstraintViewModel(string newConstraintName, string rawExpression)
            : base(newConstraintName)
        {
            this.Expression = new ConstraintExpressionViewModel(rawExpression);
            this.PopulateConnectors();
        }

        public ConstraintViewModel(string newConstraintName)
            : base(newConstraintName)
        {
            this.Expression = new ConstraintExpressionViewModel();
            this.PopulateConnectors();
        }

        /// <summary>
        /// Gets or sets the constraint expression.
        /// </summary>
        public ConstraintExpressionViewModel Expression { get; private set; }

        /// <summary>
        /// Gets whether the expression is a valid expression.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Expression.Text);
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
