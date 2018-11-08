using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Validators;

namespace Workbench.ViewModels
{
    public class AggregateVariableModelItemViewModel : VariableModelItemViewModel
    {
        private readonly IWindowManager _windowManager;

        public AggregateVariableModelItemViewModel(AggregateVariableModel theAggregateVariableModel, IWindowManager theWindowManager)
            : base(theAggregateVariableModel)
        {
            Contract.Requires<ArgumentNullException>(theAggregateVariableModel != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);

            Validator = new AggregateVariableModelItemViewModelValidator();
            AggregateVariable = theAggregateVariableModel;
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
            get { return AggregateVariable.AggregateCount; }
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
            var result = _windowManager.ShowDialog(aggregateVariableEditorViewModel);
            if (!result.HasValue) return;
            DisplayName = AggregateVariable.Name.Text = aggregateVariableEditorViewModel.VariableName;
            DomainExpressionText = AggregateVariable.DomainExpression.Text = aggregateVariableEditorViewModel.DomainExpression;
            VariableCount = aggregateVariableEditorViewModel.Size;
            AggregateVariable.Resize(aggregateVariableEditorViewModel.Size);
        }
    }
}