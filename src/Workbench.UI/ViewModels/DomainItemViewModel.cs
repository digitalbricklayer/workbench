using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for a domain.
    /// </summary>
    public sealed class DomainItemViewModel : ItemViewModel
    {
        private string _expressionText;

        public DomainItemViewModel(DomainModel theDomain)
            : base(theDomain)
        {
            Contract.Requires<ArgumentNullException>(theDomain != null);
            Domain = theDomain;
            DisplayName = theDomain.Name;
            ExpressionText = theDomain.Expression.Text;
        }

        public DomainModel Domain { get; set; }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public string ExpressionText
        {
            get { return this._expressionText; }
            set
            {
                Set(ref this._expressionText, value);
            }
        }
    }
}
