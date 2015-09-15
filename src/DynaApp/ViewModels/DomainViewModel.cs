using System;
using System.Windows;
using System.Windows.Input;
using DynaApp.Models;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for a domain.
    /// </summary>
    public sealed class DomainViewModel : GraphicViewModel
    {
        private DomainModel model;

        /// <summary>
        /// Initialize a new domain with a name and raw domain expression.
        /// </summary>
        /// <param name="newDomainName">Domain name.</param>
        /// <param name="rawExpression">Raw domain expression.</param>
        public DomainViewModel(string newDomainName, string rawExpression)
            : this(newDomainName)
        {
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
            this.Expression = new DomainExpressionViewModel(rawExpression);
        }

        /// <summary>
        /// Initialize a domain with a name and location.
        /// </summary>
        /// <param name="newDomainName">Domain name.</param>
        /// <param name="newDomainLocation">Location.</param>
        public DomainViewModel(string newDomainName, Point newDomainLocation)
            : this(newDomainName)
        {
            this.X = newDomainLocation.X;
            this.Y = newDomainLocation.Y;
        }

        /// <summary>
        /// Initialize a domain with a name.
        /// </summary>
        /// <param name="newDomainName">New domain name.</param>
        public DomainViewModel(string newDomainName)
            : base(newDomainName)
        {
            this.Model = new DomainModel();
            this.Expression = new DomainExpressionViewModel();
        }

        /// <summary>
        /// Initialize a domain with default values.
        /// </summary>
        public DomainViewModel()
        {
            this.Expression = new DomainExpressionViewModel();
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

        /// <summary>
        /// Gets or sets the domain model.
        /// </summary>
        public new DomainModel Model
        {
            get { return this.model; }
            set
            {
                base.Model = value;
                this.model = value;
            }
        }
    }
}
