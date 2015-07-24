using System;
using System.Collections.Generic;
using System.Linq;
using DynaApp.Entities;
using DynaApp.Models;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for a domain.
    /// </summary>
    public sealed class DomainViewModel : GraphicViewModel
    {
        /// <summary>
        /// Initialize a new domain with a new name and raw domain expression.
        /// </summary>
        /// <param name="newDomainName">New domain name.</param>
        /// <param name="rawExpression">Raw domain expression.</param>
        public DomainViewModel(string newDomainName, string rawExpression)
            : this(newDomainName)
        {
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
            this.Expression = new DomainExpressionViewModel(rawExpression);
        }

        /// <summary>
        /// Initialize a variable with a new name.
        /// </summary>
        /// <param name="newDomainName">New domain name.</param>
        public DomainViewModel(string newDomainName)
            : base(newDomainName)
        {
            this.Expression = new DomainExpressionViewModel();
            this.PopulateConnectors();
        }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public DomainExpressionViewModel Expression { get; set; }

        /// <summary>
        /// Get whether the domain expression is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Expression.Text);
            }
        }

        private void PopulateConnectors()
        {
            this.Connectors.Add(new ConnectorViewModel());
            this.Connectors.Add(new ConnectorViewModel());
            this.Connectors.Add(new ConnectorViewModel());
            this.Connectors.Add(new ConnectorViewModel());
        }

        public static DomainViewModel For(DomainModel domain)
        {
            return new DomainViewModel(domain.Name)
            {
                
            };
        }

        public static IEnumerable<DomainViewModel> For(IEnumerable<DomainModel> domains)
        {
            return domains.Select(For).ToList();
        }
    }
}
