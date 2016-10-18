using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the all different constraint expression editor.
    /// </summary>
    public sealed class AllDifferentConstraintExpressionViewModel : PropertyChangedBase
    {
        private bool isExpressionEditing;

        public AllDifferentConstraintExpressionViewModel(AllDifferentConstraintExpressionModel theExpressionModel)
        {
            Contract.Requires<ArgumentNullException>(theExpressionModel != null);
            Model = theExpressionModel;
        }

        /// <summary>
        /// Gets or sets the all different constraint expression model.
        /// </summary>
        public AllDifferentConstraintExpressionModel Model { get; set; }

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
