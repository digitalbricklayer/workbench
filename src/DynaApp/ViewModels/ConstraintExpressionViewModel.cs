using System;
using DynaApp.Entities;
using DynaApp.Models;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// A constraint expression view model.
    /// </summary>
    public sealed class ConstraintExpressionViewModel : AbstractViewModel
    {
        private string text;

        /// <summary>
        /// Initialize a constraint expression with a raw expression.
        /// </summary>
        public ConstraintExpressionViewModel(string rawExpression)
        {
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
            this.Text = rawExpression;
        }

        /// <summary>
        /// Initialize a constraint expression with default values.
        /// </summary>
        public ConstraintExpressionViewModel()
        {
            this.Text = string.Empty;
        }

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
                OnPropertyChanged();
            }
        }
    }
}
