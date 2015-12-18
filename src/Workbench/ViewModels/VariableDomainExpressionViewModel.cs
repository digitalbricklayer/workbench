using System;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for a variable domain expression.
    /// </summary>
    public sealed class VariableDomainExpressionViewModel : PropertyChangedBase
    {
        private bool isExpressionEditing;

        /// <summary>
        /// Initialize a variable domain expression with an expression.
        /// </summary>
        /// <param name="theExpressionModel">Variable domain expression model.</param>
        public VariableDomainExpressionViewModel(VariableDomainExpressionModel theExpressionModel)
        {
            if (theExpressionModel == null)
                throw new ArgumentException("theExpressionModel");
            this.Model = theExpressionModel;
        }

        /// <summary>
        /// Initialize a variable domain expression with a raw expression.
        /// </summary>
        /// <param name="theRawExpressionModel">Raw variable domain expression.</param>
        public VariableDomainExpressionViewModel(string theRawExpressionModel)
        {
            this.Model = new VariableDomainExpressionModel(theRawExpressionModel);
        }

        /// <summary>
        /// Gets or sets the variable domain expression model.
        /// </summary>
        public VariableDomainExpressionModel Model { get; set; }

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
                return new CommandHandler(() => this.IsExpressionEditing = true);
            }
        }
    }
}
