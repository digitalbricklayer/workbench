using System;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Messages;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for a variable.
    /// </summary>
    public class VariableViewModel : GraphicViewModel
    {
        private VariableModel model;
        protected readonly IEventAggregator eventAggregator;
        protected VariableDomainExpressionViewModel domainExpression;

        /// <summary>
        /// Initialize the variable view model with the variable model and event aggregator.
        /// </summary>
        /// <param name="theVariableModel">Variable model.</param>
        /// <param name="theEventAggregator">Event aggregator.</param>
        public VariableViewModel(VariableModel theVariableModel, IEventAggregator theEventAggregator)
            : base(theVariableModel)
        {
            if (theVariableModel == null)
                throw new ArgumentNullException("theVariableModel");

            if (theEventAggregator == null)
                throw new ArgumentNullException("theEventAggregator");

            this.Model = theVariableModel;
            this.DomainExpression = new VariableDomainExpressionViewModel(this.Model.DomainExpression);
            this.eventAggregator = theEventAggregator;
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

        /// <summary>
        /// Hook called when a graphic is renamed.
        /// </summary>
        protected override void OnRename(string oldVariableName)
        {
            base.OnRename(oldVariableName);
            var variableRenamedMessage = new VariableRenamedMessage(oldVariableName, this);
            this.eventAggregator.PublishOnUIThread(variableRenamedMessage);
        }
    }
}
