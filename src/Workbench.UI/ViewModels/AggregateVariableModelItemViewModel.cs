using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Messages;
using Workbench.Validators;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Aggregate variable item inside the model editor.
    /// </summary>
    public class AggregateVariableModelItemViewModel : VariableModelItemViewModel
    {
        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        public AggregateVariableModelItemViewModel(AggregateVariableModel theAggregateVariableModel, IWindowManager theWindowManager, IEventAggregator theEventAggregator)
            : base(theAggregateVariableModel)
        {
            Validator = new AggregateVariableModelItemViewModelValidator();
            AggregateVariable = theAggregateVariableModel;
            _eventAggregator = theEventAggregator;
            _windowManager = theWindowManager;
            Variables = new BindableCollection<VariableModelItemViewModel>();
        }

        /// <summary>
        /// Gets the aggregate variable model.
        /// </summary>
        public AggregateVariableModel AggregateVariable { get; private set; }

        /// <summary>
        /// Gets the variables inside the aggregate.
        /// </summary>
        public IObservableCollection<VariableModelItemViewModel> Variables { get; private set; }

        /// <summary>
        /// Gets or sets the number of variables in the aggregate variable.
        /// </summary>
        public int VariableCount
        {
            get => AggregateVariable.AggregateCount;
            set
            {
                AggregateVariable.Resize(value);
                NotifyOfPropertyChange();
            }
        }

        public override void Edit()
        {
            var aggregateVariableEditorViewModel = new AggregateVariableEditorViewModel();
            aggregateVariableEditorViewModel.VariableName = AggregateVariable.Name;
            aggregateVariableEditorViewModel.DomainExpression = AggregateVariable.DomainExpression.Text;
            aggregateVariableEditorViewModel.Size = AggregateVariable.AggregateCount;
            var oldName = AggregateVariable.Name.Text;
            var result = _windowManager.ShowDialog(aggregateVariableEditorViewModel);
            if (!result.GetValueOrDefault()) return;
            DisplayName = AggregateVariable.Name.Text = aggregateVariableEditorViewModel.VariableName;
            DomainExpressionText = AggregateVariable.DomainExpression.Text = aggregateVariableEditorViewModel.DomainExpression;
            VariableCount = aggregateVariableEditorViewModel.Size;
            AggregateVariable.Resize(aggregateVariableEditorViewModel.Size);
            if (oldName != aggregateVariableEditorViewModel.VariableName)
            {
                _eventAggregator.PublishOnUIThread(new VariableRenamedMessage(new ModelName(oldName), AggregateVariable));
            }
        }
    }
}