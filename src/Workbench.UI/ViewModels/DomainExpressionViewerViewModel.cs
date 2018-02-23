using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Domain expression viewer view model.
    /// </summary>
    public class DomainExpressionViewerViewModel : Screen
    {
        private string text;

        /// <summary>
        /// Initialize a domain expression viewer with a domain expression.
        /// </summary>
        /// <param name="theDomainExpression">Domain expression.</param>
        public DomainExpressionViewerViewModel(DomainExpressionModel theDomainExpression)
        {
            Contract.Requires<ArgumentNullException>(theDomainExpression != null);
            Text = theDomainExpression.Text;
        }

        /// <summary>
        /// Gets or sets the domain expression text.
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                NotifyOfPropertyChange();
            }
        }
    }
}