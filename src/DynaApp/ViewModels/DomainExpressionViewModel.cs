using System;
using DynaApp.Models;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// A domain expression view model.
    /// </summary>
    public sealed class DomainExpressionViewModel : AbstractViewModel
    {
        private string text;

        /// <summary>
        /// Initialize a domain expression with a raw expression.
        /// </summary>
        /// <param name="rawExpression">Raw expression text.</param>
        public DomainExpressionViewModel(string rawExpression)
        {
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
            this.Model = new DomainExpressionModel();
            this.Text = rawExpression;
        }

        /// <summary>
        /// Initialize a domain expression with default values.
        /// </summary>
        public DomainExpressionViewModel()
        {
            this.Model = new DomainExpressionModel();
            this.Text = string.Empty;
        }

        /// <summary>
        /// Gets or sets the domain expression model.
        /// </summary>
        public DomainExpressionModel Model { get; set; }

        /// <summary>
        /// Gets or sets the domain expression text.
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
    }
}
