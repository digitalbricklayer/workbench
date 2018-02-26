using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class AggregateVariableViewerViewModel : VariableViewerViewModel
    {
        private VariableDomainExpressionViewModel domainExpression;

        public AggregateVariableViewerViewModel(AggregateVariableGraphicModel theAggregateVariableGraphic)
            : base(theAggregateVariableGraphic)
        {
            AggregateVariableGraphic = theAggregateVariableGraphic;
            DomainExpression = new VariableDomainExpressionViewModel(AggregateVariableGraphic.DomainExpression);
        }

        public AggregateVariableGraphicModel AggregateVariableGraphic { get; set; }

        /// <summary>
        /// Gets the variables inside the aggregate.
        /// </summary>
        public IObservableCollection<VariableEditorViewModel> Variables { get; private set; }

        /// <summary>
        /// Gets or sets the number of variables in the aggregate variable.
        /// </summary>
        public int VariableCount
        {
            get
            {
                return AggregateVariableGraphic.AggregateCount;
            }
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
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets whether the variable is an aggregate.
        /// </summary>
        public bool IsAggregate => true;

    }
}