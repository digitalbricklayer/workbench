using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// A domain expression editor view model.
    /// </summary>
    public sealed class DomainExpressionEditorViewModel : Screen
    {
        private bool isExpressionEditing;

        /// <summary>
        /// Initialize a domain expression editor with a domain expression.
        /// </summary>
        /// <param name="theDomainExpression">Domain expression.</param>
        public DomainExpressionEditorViewModel(DomainExpressionModel theDomainExpression)
        {
            Contract.Requires<ArgumentNullException>(theDomainExpression != null);
            Model = theDomainExpression;
        }

        /// <summary>
        /// Initialize a domain expression with default values.
        /// </summary>
        public DomainExpressionEditorViewModel()
        {
            Model = new DomainExpressionModel();
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
                return new CommandHandler(() => IsExpressionEditing = true, _ => true);
            }
        }
    }
}
