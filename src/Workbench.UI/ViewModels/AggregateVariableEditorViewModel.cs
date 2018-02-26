using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class AggregateVariableEditorViewModel : VariableEditorViewModel
    {
        public AggregateVariableEditorViewModel(AggregateVariableGraphicModel theGraphicModel, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theGraphicModel, theEventAggregator, theDataService, theViewModelService)
        {
            Variables = new BindableCollection<VariableEditorViewModel>();
            Model = AggregateVariableGraphic = theGraphicModel;
        }

        public AggregateVariableGraphicModel AggregateVariableGraphic { get; set; }

        /// <summary>
        /// Gets the aggregate variable name.
        /// </summary>
        public override string Name
        {
            get { return base.Name; }
            set
            {
                this.Model.Name = value;
                for (var i = 1; i <= this.Variables.Count; i++)
                    this.Variables[i - 1].Name = this.Name + i;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the aggregate variable model.
        /// </summary>
        public new AggregateVariableGraphicModel Model { get; private set; }

        /// <summary>
        /// Gets the variables inside the aggregate.
        /// </summary>
        public IObservableCollection<VariableEditorViewModel> Variables { get; private set; }

        /// <summary>
        /// Gets or sets the number of variables in the aggregate variable.
        /// </summary>
        public int VariableCount
        {
            get { return Model.AggregateCount; }
            set
            {
                Resize(value);
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public new VariableDomainExpressionEditorViewModel DomainExpression
        {
            get
            {
                return base.DomainExpression;
            }
            set
            {
                base.DomainExpression = value;
                this.Model.DomainExpression = value.Model;
                foreach (var variable in Variables)
                {
                    variable.DomainExpression = value;
                    variable.DomainExpression = value;
                }
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets whether the variable is an aggregate.
        /// </summary>
        public override bool IsAggregate => true;

        private void Resize(int newSize)
        {
            this.Model.Resize(newSize);
            if (newSize > this.Variables.Count)
                this.GrowBy(newSize - this.Variables.Count);
            else
                this.ShrinkBy(this.Variables.Count - newSize);
        }

        private void GrowBy(int variablesToIncreaseBy)
        {
            for (var i = 0; i < variablesToIncreaseBy; i++)
            {
                var newVariable = new SingletonVariableEditorViewModel(new SingletonVariableGraphicModel(new SingletonVariableModel(Model.Variable.Model, new ModelName(Name + i))),
                                                                 this.eventAggregator, this.dataService, this.viewModelService);
                newVariable.DomainExpression = this.DomainExpression;
                this.Variables.Add(newVariable);
            }
        }

        private void ShrinkBy(int variablesToShrinkBy)
        {
            for (var i = 0; i < variablesToShrinkBy; i++)
                this.Variables.RemoveAt(this.Variables.Count - 1);
        }
    }
}