using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// A constraint expression editor view model.
    /// </summary>
    public sealed class ConstraintExpressionEditorViewModel : PropertyChangedBase
    {
        private bool isExpressionEditing;

        /// <summary>
        /// Initialize a constraint expression with an expression model.
        /// </summary>
        /// <param name="theExpressionModel">Constraint expression model.</param>
        public ConstraintExpressionEditorViewModel(ConstraintExpressionModel theExpressionModel)
        {
            Contract.Requires<ArgumentNullException>(theExpressionModel != null);
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
            get { return Model.Text; }
            set
            {
                if (Model.Text == value) return;
                Model.Text = value;
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
                return new CommandHandler(() => IsExpressionEditing = true);
            }
        }
    }
}
