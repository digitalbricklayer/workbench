using System;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// A constraint expression view model.
    /// </summary>
    public sealed class ConstraintExpressionViewModel : PropertyChangedBase
    {
        private bool isExpressionEditing;

        /// <summary>
        /// Initialize a constraint expression with an expression model.
        /// </summary>
        /// <param name="theExpressionModel">Constraint expression model.</param>
        public ConstraintExpressionViewModel(ConstraintExpressionModel theExpressionModel)
        {
            if (theExpressionModel == null)
                throw new ArgumentNullException("theExpressionModel");
            this.Model = theExpressionModel;
        }

        /// <summary>
        /// Gets or sets the constraint expression model.
        /// </summary>
        public ConstraintExpressionModel Model { get; set; }

        /// <summary>
        /// Gets or sets the constraint expression text.
        /// </summary>
        public string Text
        {
            get { return this.Model.Text; }
            set
            {
                if (this.Model.Text == value) return;
                this.Model.Text = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets whether the expression is being edited.
        /// </summary>
        public bool IsExpressionEditing
        {
            get { return this.isExpressionEditing; }
            set
            {
                if (this.isExpressionEditing == value) return;
                this.isExpressionEditing = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the domain expression edit command.
        /// </summary>
        public ICommand EditExpressionCommand
        {
            get
            {
                return new CommandHandler(() => this.IsExpressionEditing = true);
            }
        }
    }
}
