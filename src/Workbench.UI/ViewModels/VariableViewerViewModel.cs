using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public abstract class VariableViewerViewModel : ViewerViewModel
    {
        private VariableDomainExpressionViewerViewModel domainExpression;

        protected VariableViewerViewModel(VariableGraphicModel theVariableModel)
            : base(theVariableModel)
        {
            VariableGraphic = theVariableModel;
            this.domainExpression = new VariableDomainExpressionViewerViewModel(theVariableModel.DomainExpression);
        }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public VariableDomainExpressionViewerViewModel DomainExpression
        {
            get
            {
                return this.domainExpression;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.domainExpression = value;
                NotifyOfPropertyChange();
            }
        }

        public VariableGraphicModel VariableGraphic { get; set; }

        /// <summary>
        /// Gets whether the variable is an aggregate.
        /// </summary>
        public abstract bool IsAggregate { get; }
    }
}