using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class VariableEditorViewModel : EditorViewModel
    {
        private VariableDomainExpressionViewModel domainExpression;

        public VariableEditorViewModel(VariableGraphicModel theGraphicModel, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theGraphicModel, theEventAggregator, theDataService, theViewModelService)
        {
            VariableGraphic = theGraphicModel;
            this.domainExpression = new VariableDomainExpressionViewModel(theGraphicModel.DomainExpression);
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
                Contract.Requires<ArgumentNullException>(value != null);
                this.domainExpression = value;
                if (this.Model != null)
                    VariableGraphic.DomainExpression = this.domainExpression.Model;
                NotifyOfPropertyChange();
            }
        }

        public VariableGraphicModel VariableGraphic { get; set; }
        public bool IsAggregate { get; set; }
    }
}