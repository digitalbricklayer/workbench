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

        public VariableViewModel(VariableModel theVariableModel)
            : base(theVariableModel)
        {
            this.Model = theVariableModel;
            this.DomainExpression = new VariableDomainExpressionViewModel(this.Model.DomainExpression);
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
