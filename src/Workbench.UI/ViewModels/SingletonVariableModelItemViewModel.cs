using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Validators;

namespace Workbench.ViewModels
{
    public sealed class SingletonVariableModelItemViewModel : VariableModelItemViewModel
    {
        private readonly IWindowManager _windowManager;

        public SingletonVariableModelItemViewModel(SingletonVariableModel theSingletonVariableModel, IWindowManager theWindowManager)
            : base(theSingletonVariableModel)
        {
            Contract.Requires<ArgumentNullException>(theSingletonVariableModel != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);

            Validator = new SingletonVariableModelItemViewModelValidator();
            SingletonVariable = theSingletonVariableModel;
            _windowManager = theWindowManager;
        }

        /// <summary>
        /// Gets the singleton variable model.
        /// </summary>
        public SingletonVariableModel SingletonVariable { get; }

        public override void Edit()
        {
            var singletonVariableEditorViewModel = new SingletonVariableEditorViewModel();
            singletonVariableEditorViewModel.VariableName = SingletonVariable.Name;
            singletonVariableEditorViewModel.DomainExpression = SingletonVariable.DomainExpression.Text;
            var result = _windowManager.ShowDialog(singletonVariableEditorViewModel);
            if (!result.HasValue) return;
            DisplayName = SingletonVariable.Name.Text = singletonVariableEditorViewModel.VariableName;
            DomainExpressionText = SingletonVariable.DomainExpression.Text = singletonVariableEditorViewModel.DomainExpression;
        }
    }
}