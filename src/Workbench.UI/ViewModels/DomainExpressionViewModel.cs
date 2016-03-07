using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// A domain expression view model.
    /// </summary>
    public sealed class DomainExpressionViewModel : PropertyChangedBase
    {
        private bool isExpressionEditing;

        /// <summary>
        /// Initialize a domain expression with a raw expression.
        /// </summary>
        /// <param name="rawExpression">Raw expression text.</param>
        public DomainExpressionViewModel(string rawExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(rawExpression));
            this.Model = new DomainExpressionModel();
        }

        /// <summary>
        /// Initialize a domain expression with a domain expression.
        /// </summary>
        /// <param name="theDomainExpression">Domain expression.</param>
        public DomainExpressionViewModel(DomainExpressionModel theDomainExpression)
        {
            if (theDomainExpression == null)
                throw new ArgumentNullException("theDomainExpression");
            this.Model = theDomainExpression;
        }

        /// <summary>
        /// Initialize a domain expression with default values.
        /// </summary>
        public DomainExpressionViewModel()
        {
            this.Model = new DomainExpressionModel();
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
                return new CommandHandler(() => this.IsExpressionEditing = true, _ => true);
            }
        }
    }
}
