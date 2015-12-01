using System;
using System.Windows;
using Dyna.Core.Models;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for a variable.
    /// </summary>
    public class VariableViewModel : GraphicViewModel
    {
        private VariableModel model;
        protected VariableDomainExpressionViewModel domainExpression;

        /// <summary>
        /// Initialize a variable with a name, location and domain expression.
        /// </summary>
        public VariableViewModel(string newName, Point newLocation, VariableDomainExpressionViewModel newDomainExpression)
            : base(newName, newLocation)
        {
            if (newDomainExpression == null)
                throw new ArgumentNullException("newDomainExpression");

            this.DomainExpression = newDomainExpression;
            this.Model = new VariableModel(newName, this.DomainExpression.Model);
        }

        /// <summary>
        /// Initialize a variable with a name and domain expression.
        /// </summary>
        public VariableViewModel(string newName, VariableDomainExpressionViewModel newDomainExpression)
            : base(newName)
        {
            if (newDomainExpression == null)
                throw new ArgumentNullException("newDomainExpression");

            this.DomainExpression = newDomainExpression;
            this.Model = new VariableModel(newName, this.DomainExpression.Model);
        }

        /// <summary>
        /// Initialize a variable with a name and location.
        /// </summary>
        public VariableViewModel(string newName, Point newLocation)
            : base(newName, newLocation)
        {
            this.DomainExpression = new VariableDomainExpressionViewModel();
            this.Model = new VariableModel(newName, this.DomainExpression.Model);
        }

        /// <summary>
        /// Initialize a variable with a name.
        /// </summary>
        public VariableViewModel(string newName)
            : base(newName)
        {
            this.DomainExpression = new VariableDomainExpressionViewModel();
            this.Model = new VariableModel(newName, this.DomainExpression.Model);
        }

        /// <summary>
        /// Initialize a variable with default values.
        /// </summary>
        public VariableViewModel()
            : this("New variable")
        {
            this.DomainExpression = new VariableDomainExpressionViewModel();
            this.Model = new VariableModel("New variable", this.DomainExpression.Model);
        }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public VariableDomainExpressionViewModel DomainExpression
        {
            get
            {
                return this.domainExpression;
            }
            set
            {
                this.domainExpression = value;
                if (this.Model != null)
                    this.Model.DomainExpression = this.domainExpression.Model;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the variable model.
        /// </summary>
        public new VariableModel Model
        {
            get { return this.model; }
            set
            {
                base.Model = value;
                this.model = value;
            }
        }

        /// <summary>
        /// Gets whether the variable is an aggregate.
        /// </summary>
        public virtual bool IsAggregate
        {
            get
            {
                return false;
            }
        }
    }
}
