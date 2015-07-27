using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DynaApp.Models;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for a constraint.
    /// </summary>
    public class ConstraintViewModel : GraphicViewModel
    {
        /// <summary>
        /// Initialize a constraint with a name and raw constraint expression.
        /// </summary>
        /// <param name="newConstraintName">New constraint name.</param>
        /// <param name="rawExpression">Raw constraint expression.</param>
        public ConstraintViewModel(string newConstraintName, string rawExpression)
            : base(newConstraintName)
        {
            this.Expression = new ConstraintExpressionViewModel(rawExpression);
            this.PopulateConnectors();
        }

        /// <summary>
        /// Initialize a constraint with a name and a location.
        /// </summary>
        /// <param name="newConstraintName">New constraint name.</param>
        /// <param name="newLocation">New constraint location.</param>
        public ConstraintViewModel(string newConstraintName, Point newLocation)
            : this(newConstraintName)
        {
            this.X = newLocation.X;
            this.Y = newLocation.Y;
        }

        /// <summary>
        /// Initialize a constraint with a constraint name.
        /// </summary>
        /// <param name="newConstraintName">New constraint name.</param>
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
