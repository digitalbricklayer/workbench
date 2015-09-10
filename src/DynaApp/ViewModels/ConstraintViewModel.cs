using System.Windows;
using System.Windows.Input;
using DynaApp.Models;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for a constraint.
    /// </summary>
    public class ConstraintViewModel : GraphicViewModel
    {
        private ConstraintModel model;

        /// <summary>
        /// Initialize a constraint with a name and raw constraint expression.
        /// </summary>
        /// <param name="newConstraintName">New constraint name.</param>
        /// <param name="rawExpression">Raw constraint expression.</param>
        public ConstraintViewModel(string newConstraintName, string rawExpression)
            : base(newConstraintName)
        {
            this.Expression = new ConstraintExpressionViewModel(rawExpression);
            this.Model = new ConstraintModel();
        }

        /// <summary>
        /// Initialize a constraint with a name and a location.
        /// </summary>
        /// <param name="newConstraintName">New constraint name.</param>
        /// <param name="newLocation">New constraint location.</param>
        public ConstraintViewModel(string newConstraintName, Point newLocation)
            : this(newConstraintName)
        {
            this.Model = new ConstraintModel();
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
            this.Model = new ConstraintModel();
        }

        /// <summary>
        /// Initialize a constraint with default values.
        /// </summary>
        public ConstraintViewModel()
        {
            this.Expression = new ConstraintExpressionViewModel();
            this.Model = new ConstraintModel();
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

        /// <summary>
        /// Gets or sets the constraint model.
        /// </summary>
        public new ConstraintModel Model
        {
            get { return this.model; }
            set
            {
                base.Model = value;
                this.model = value;
            }
        }

        /// <summary>
        /// Gets the edit constraint name command.
        /// </summary>
        public ICommand EditNameCommand
        {
            get
            {
                return new CommandHandler(() => this.IsNameEditing = true, true);
            }
        }
    }
}
