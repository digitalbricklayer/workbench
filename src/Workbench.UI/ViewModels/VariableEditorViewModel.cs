using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Messages;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public abstract class VariableEditorViewModel : EditorViewModel
    {
        private VariableDomainExpressionViewModel domainExpression;

        protected VariableEditorViewModel(VariableGraphicModel theGraphicModel, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
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

        /// <summary>
        /// Gets whether the variable is an aggregate.
        /// </summary>
        public abstract bool IsAggregate { get; }

        /// <summary>
        /// Hook called when a variable is renamed.
        /// </summary>
        protected override void OnRename(string oldVariableName)
        {
            base.OnRename(oldVariableName);
            var variableRenamedMessage = new VariableRenamedMessage(oldVariableName, this);
            this.eventAggregator.PublishOnUIThread(variableRenamedMessage);
        }
    }
}