using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for a variable domain expression.
    /// </summary>
    public sealed class VariableDomainExpressionViewerViewModel : PropertyChangedBase
    {
        private bool isExpressionEditing;
        private string text;

        /// <summary>
        /// Initialize a variable domain expression viewer with an expression.
        /// </summary>
        /// <param name="theExpressionModel">Variable domain expression model.</param>
        public VariableDomainExpressionViewerViewModel(VariableDomainExpressionModel theExpressionModel)
        {
            Contract.Requires<ArgumentNullException>(theExpressionModel != null);
            this.Model = theExpressionModel;
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
            get { return this.text; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.text = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
