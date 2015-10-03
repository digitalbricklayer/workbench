using System;
using System.Windows.Input;
using Dyna.Core.Models;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// A constraint expression view model.
    /// </summary>
    public sealed class ConstraintExpressionViewModel : AbstractViewModel
    {
        private string text;
        private bool isExpressionEditing;

        /// <summary>
        /// Initialize a constraint expression with a raw expression.
        /// </summary>
        public ConstraintExpressionViewModel(string rawExpression)
        {
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
            this.Model = new ConstraintExpressionModel();
            this.Text = rawExpression;
        }

        /// <summary>
        /// Initialize a constraint expression with default values.
        /// </summary>
        public ConstraintExpressionViewModel()
        {
            this.Model = new ConstraintExpressionModel();
            this.Text = string.Empty;
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
            get { return this.text; }
            set
            {
                if (this.text == value) return;
                this.text = value;
                this.Model.Text = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the domain expression edit command.
        /// </summary>
        public ICommand EditExpressionCommand
        {
            get
            {
                return new CommandHandler(() => this.IsExpressionEditing = true, true);
            }
        }
    }
}
