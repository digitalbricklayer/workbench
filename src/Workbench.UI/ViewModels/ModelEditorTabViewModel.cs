using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the model editor.
    /// </summary>
    public sealed class ModelEditorTabViewModel : Conductor<ModelItemViewModel>.Collection.OneActive, IWorkspaceTabViewModel
    {
        private ICommand _addSingletonVariableCommand;
        private ICommand _addAggregateVariableCommand;
        private readonly IAppRuntime _appRuntime;
        private readonly IWindowManager _windowManager;
        private ICommand _addDomainCommand;
        private ICommand _addExpressionConstraintCommand;
        private ICommand _addAllDifferentConstraintCommand;
        private ICommand _editCommand;
        private CommandHandler _deleteCommand;

        /// <summary>
        /// Initialize a solution designer view model with default values.
        /// </summary>
        public ModelEditorTabViewModel(IAppRuntime theAppRuntime, IDataService theDataService, IWindowManager theWindowManager)
        {
            Contract.Requires<ArgumentNullException>(theAppRuntime != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);

            _appRuntime = theAppRuntime;
            _windowManager = theWindowManager;
            DisplayName = "Model";
            ModelModel = theDataService.GetWorkspace().Model;
            Variables = new BindableCollection<VariableModelItemViewModel>();
            Domains = new BindableCollection<DomainModelItemViewModel>();
            Constraints = new BindableCollection<ConstraintModelItemViewModel>();
            AddSingletonVariableCommand =  new CommandHandler(AddSingletonVariableAction);
            AddAggregateVariableCommand = new CommandHandler(AddAggregateVariableAction);
            AddDomainCommand = new CommandHandler(AddDomainAction);
            AddAllDifferentConstraintCommand = new CommandHandler(AddAllDifferentConstraintAction);
            AddExpressionConstraintCommand = new CommandHandler(AddExpressionConstraintAction);
            EditCommand = new CommandHandler(EditAction, IsItemSelected);
            DeleteCommand = new CommandHandler(DeleteAction, IsItemSelected);
        }

        /// <summary>
        /// Gets the work area view model.
        /// </summary>
        public WorkspaceViewModel Workspace
        {
            get { return _appRuntime.Workspace; }
        }

        /// <summary>
        /// Gets or sets the model model.
        /// </summary>
        public ModelModel ModelModel { get; set; }

        /// <summary>
        /// Gets the collection of variables.
        /// </summary>
        public IObservableCollection<VariableModelItemViewModel> Variables { get; private set; }

        /// <summary>
        /// Gets the collection of domains.
        /// </summary>
        public IObservableCollection<DomainModelItemViewModel> Domains { get; private set; }

        /// <summary>
        /// Gets the collection of constraints.
        /// </summary>
        public IObservableCollection<ConstraintModelItemViewModel> Constraints { get; private set; }

        /// <summary>
        /// Gets or sets the Add singleton variable command.
        /// </summary>
        public ICommand AddSingletonVariableCommand
        {
            get => _addSingletonVariableCommand;
            set
            {
                _addSingletonVariableCommand = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the Add singleton variable command.
        /// </summary>
        public ICommand AddAggregateVariableCommand
        {
            get => _addAggregateVariableCommand;
            set
            {
                _addAggregateVariableCommand = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the Add singleton variable command.
        /// </summary>
        public ICommand AddDomainCommand
        {
            get => _addDomainCommand;
            set
            {
                _addDomainCommand = value;
                NotifyOfPropertyChange();
            }
        }

        public ICommand AddExpressionConstraintCommand
        {
            get => _addExpressionConstraintCommand;
            set
            {
                _addExpressionConstraintCommand = value;
                NotifyOfPropertyChange();
            }
        }

        public ICommand AddAllDifferentConstraintCommand
        {
            get => _addAllDifferentConstraintCommand;
            set
            {
                _addAllDifferentConstraintCommand = value;
                NotifyOfPropertyChange();
            }
        }

        public ICommand EditCommand
        {
            get => _editCommand;
            set
            {
                _editCommand = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the delete command.
        /// </summary>
        public CommandHandler DeleteCommand
        {
            get => _deleteCommand;
            set
            {
                _deleteCommand = value; 
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Get whether the currently selected tab be closed by the user.
        /// </summary>
        public bool CloseTabIsVisible => false;

        /// <summary>
        /// Add a new singleton variable.
        /// </summary>
        /// <param name="newVariableModel">New variable.</param>
        public void AddSingletonVariable(SingletonVariableModel newVariableModel)
        {
            Contract.Requires<ArgumentNullException>(newVariableModel != null);

            var newSingletonVariableItem = new SingletonVariableModelItemViewModel(newVariableModel, _windowManager);
            FixupSingletonVariable(newSingletonVariableItem);
            ModelModel.AddVariable(newVariableModel);
        }

        /// <summary>
        /// Add a new aggregate variable to the model.
        /// </summary>
        /// <param name="newVariableModel">New variable.</param>
        public void AddAggregateVariable(AggregateVariableModel newVariableModel)
        {
            Contract.Requires<ArgumentNullException>(newVariableModel != null);

            var newAggregateVariableItem = new AggregateVariableModelItemViewModel(newVariableModel, _windowManager);
            FixupAggregateVariable(newAggregateVariableItem);
            ModelModel.AddVariable(newVariableModel);
        }

        /// <summary>
        /// Add a new domain to the model.
        /// </summary>
        /// <param name="newDomainModel">New domain.</param>
        public void AddDomain(DomainModel newDomainModel)
        {
            Contract.Requires<ArgumentNullException>(newDomainModel != null);

            var newDomainItem = new DomainModelItemViewModel(newDomainModel, _windowManager);
            FixupDomain(newDomainItem);
            AddDomainToModel(newDomainModel);
        }

        /// <summary>
        /// Add a new all different constraint to the model.
        /// </summary>
        /// <param name="newAllDifferentConstraint">New constraint.</param>
        public void AddConstraint(AllDifferentConstraintModel newAllDifferentConstraint)
        {
            Contract.Requires<ArgumentNullException>(newAllDifferentConstraint != null);

            var newConstraint = new AllDifferentConstraintModelItemViewModel(newAllDifferentConstraint, _windowManager);
            FixupConstraint(newConstraint);
            AddConstraintToModel(newConstraint);
        }

        /// <summary>
        /// Add a new expression constraint to the model.
        /// </summary>
        /// <param name="newExpressionConstraint">New constraint.</param>
        public void AddConstraint(ExpressionConstraintModel newExpressionConstraint)
        {
            Contract.Requires<ArgumentNullException>(newExpressionConstraint != null);

            var newExpressionConstraintEditor = new ExpressionConstraintModelItemViewModel(newExpressionConstraint, _windowManager);
            FixupConstraint(newExpressionConstraintEditor);
            AddConstraintToModel(newExpressionConstraintEditor);
        }

        /// <summary>
        /// Delete the variable.
        /// </summary>
        /// <param name="variableToDelete">Variable to delete.</param>
        public void DeleteVariable(VariableModelItemViewModel variableToDelete)
        {
            Contract.Requires<ArgumentNullException>(variableToDelete != null);

            Items.Remove(variableToDelete);
            Variables.Remove(variableToDelete);
            DeactivateItem(variableToDelete, close: true);
            DeleteVariableFromModel(variableToDelete);
        }

        /// <summary>
        /// Delete the domain.
        /// </summary>
        /// <param name="domainToDelete">Domain to delete.</param>
        public void DeleteDomain(DomainModelItemViewModel domainToDelete)
        {
            Contract.Requires<ArgumentNullException>(domainToDelete != null);

            Domains.Remove(domainToDelete);
            DeactivateItem(domainToDelete, close: true);
            DeleteDomainFromModel(domainToDelete);
        }

        /// <summary>
        /// Delete the constraint.
        /// </summary>
        /// <param name="constraintToDelete">Constraint to delete.</param>
        public void DeleteConstraint(ConstraintModelItemViewModel constraintToDelete)
        {
            Contract.Requires<ArgumentNullException>(constraintToDelete != null);

            Constraints.Remove(constraintToDelete);
            DeactivateItem(constraintToDelete, close: true);
            DeleteConstraintFromModel(constraintToDelete);
        }

        /// <summary>
        /// Fixes up a singleton variable view model into the model.
        /// </summary>
        /// <remarks>
        /// Used when mapping the model to a view model.
        /// </remarks>
        /// <param name="variableViewModel">Singleton variable view model.</param>
        internal void FixupSingletonVariable(SingletonVariableModelItemViewModel variableViewModel)
        {
            Contract.Requires<ArgumentNullException>(variableViewModel != null);
            Variables.Add(variableViewModel);
            ActivateItem(variableViewModel);
        }

        /// <summary>
        /// Fixes up an aggregate variable view model into the model.
        /// </summary>
        /// <remarks>
        /// Used when mapping the model to a view model.
        /// </remarks>
        /// <param name="variableViewModel">Aggregate variable view model.</param>
        internal void FixupAggregateVariable(AggregateVariableModelItemViewModel variableViewModel)
        {
            Contract.Requires<ArgumentNullException>(variableViewModel != null);
            Variables.Add(variableViewModel);
            ActivateItem(variableViewModel);
        }

        /// <summary>
        /// Fixes up a domain view model into the model.
        /// </summary>
        /// <remarks>
        /// Used when mapping the model to a view model.
        /// </remarks>
        /// <param name="domainViewModel">Domain view model.</param>
        internal void FixupDomain(DomainModelItemViewModel domainViewModel)
        {
            Contract.Requires<ArgumentNullException>(domainViewModel != null);
            Domains.Add(domainViewModel);
            ActivateItem(domainViewModel);
        }

        /// <summary>
        /// Fixes up a constraint view model into the model.
        /// </summary>
        /// <remarks>
        /// Used when mapping the model to a view model.
        /// </remarks>
        /// <param name="constraintViewModel">Constraint view model.</param>
        internal void FixupConstraint(ConstraintModelItemViewModel constraintViewModel)
        {
            Contract.Requires<ArgumentNullException>(constraintViewModel != null);
            Constraints.Add(constraintViewModel);
            ActivateItem(constraintViewModel);
        }

        /// <summary>
        /// Add a new domain to the model model.
        /// </summary>
        /// <param name="newDomainModel">New domain view model.</param>
        private void AddDomainToModel(DomainModel newDomainModel)
        {
            Contract.Assert(newDomainModel != null);

            newDomainModel.AssignIdentity();
            ModelModel.AddSharedDomain(newDomainModel);
        }

        /// <summary>
        /// Add a new all different constraint to the model model.
        /// </summary>
        /// <param name="newConstraintViewModel">New constraint view model.</param>
        private void AddConstraintToModel(AllDifferentConstraintModelItemViewModel newConstraintViewModel)
        {
            Contract.Assert(newConstraintViewModel.Constraint != null);
            ModelModel.AddConstraint(newConstraintViewModel.Constraint);
        }

        /// <summary>
        /// Add a new expression constraint to the model model.
        /// </summary>
        /// <param name="newConstraintViewModel">New constraint view model.</param>
        private void AddConstraintToModel(ExpressionConstraintModelItemViewModel newConstraintViewModel)
        {
            Contract.Assert(newConstraintViewModel.Model != null);
            newConstraintViewModel.Model.AssignIdentity();
            ModelModel.AddConstraint(newConstraintViewModel.ExpressionConstraint);
        }

        private void DeleteConstraintFromModel(ConstraintModelItemViewModel constraintToDelete)
        {
            Contract.Assert(constraintToDelete.Model != null);
            ModelModel.DeleteConstraint(constraintToDelete.Constraint);
        }

        private void DeleteVariableFromModel(VariableModelItemViewModel variableToDelete)
        {
            Contract.Assert(variableToDelete.Model != null);
            ModelModel.DeleteVariable(variableToDelete.Variable);
        }

        private void DeleteDomainFromModel(DomainModelItemViewModel domainToDelete)
        {
            Contract.Assert(domainToDelete.Model != null);
            ModelModel.DeleteDomain(domainToDelete.Domain);
        }

        private void AddSingletonVariableAction()
        {
            var singletonVariableEditorViewModel = new SingletonVariableEditorViewModel();
            var x = _windowManager.ShowDialog(singletonVariableEditorViewModel);
            if (!x.HasValue) return;
            Workspace.AddSingletonVariable(new SingletonVariableBuilder().WithName(singletonVariableEditorViewModel.VariableName)
                                                                        .WithDomain(singletonVariableEditorViewModel.DomainExpression)
                                                                        .WithModel(Workspace.WorkspaceModel.Model)
                                                                        .Build());
        }

        private void AddAggregateVariableAction()
        {
            var aggregateVariableEditorViewModel = new AggregateVariableEditorViewModel();
            var x = _windowManager.ShowDialog(aggregateVariableEditorViewModel);
            if (!x.HasValue) return;
            Workspace.AddAggregateVariable(new AggregateVariableBuilder().WithName(aggregateVariableEditorViewModel.VariableName)
                                                                        .WithDomain(aggregateVariableEditorViewModel.DomainExpression)
                                                                        .WithSize(aggregateVariableEditorViewModel.Size)
                                                                        .WithModel(Workspace.WorkspaceModel.Model)
                                                                        .Build());
        }

        private void AddDomainAction()
        {
            var domainEditorViewModel = new DomainEditorViewModel();
            var x = _windowManager.ShowDialog(domainEditorViewModel);
            if (!x.HasValue) return;
            Workspace.AddDomain(new DomainBuilder().WithName(domainEditorViewModel.DomainName)
                                                  .WithDomain(domainEditorViewModel.DomainExpression)
                                                  .Build());
        }

        private void AddExpressionConstraintAction()
        {
            var expressionConstraintEditViewModel = new ExpressionConstraintEditorViewModel();
            var x = _windowManager.ShowDialog(expressionConstraintEditViewModel);
            if (!x.HasValue) return;
            Workspace.AddExpressionConstraint(new ExpressionConstraintBuilder().WithName(expressionConstraintEditViewModel.ConstraintName)
                                                                               .WithExpression(expressionConstraintEditViewModel.ConstraintExpression)
                                                                               .Build());
        }

        private void AddAllDifferentConstraintAction()
        {
            var allDifferentConstraintEditViewModel = new AllDifferentConstraintEditorViewModel();
            var x = _windowManager.ShowDialog(allDifferentConstraintEditViewModel);
            if (!x.HasValue) return;
            Workspace.AddAllDifferentConstraint(new AllDifferentConstraintBuilder().WithName(allDifferentConstraintEditViewModel.ConstraintName)
                                                                                   .WithExpression(allDifferentConstraintEditViewModel.ConstraintExpression)
                                                                                   .Build());
        }

        private void EditAction()
        {
            Contract.Assume(ActiveItem != null);
            ActiveItem.Edit();
        }

        private void DeleteAction()
        {
            Contract.Assume(ActiveItem != null);

            switch (ActiveItem)
            {
                case VariableModelItemViewModel variableItem:
                    DeleteVariable(variableItem);
                    break;

                case ConstraintModelItemViewModel constraintItem:
                    DeleteConstraint(constraintItem);
                    break;

                case DomainModelItemViewModel domainItem:
                    DeleteDomain(domainItem);
                    break;

                default:
                    throw new NotImplementedException("Unknown item.");
            }
        }

        private bool IsItemSelected(object parameter)
        {
            return ActiveItem != null;
        }

        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(ModelModel != null);
            Contract.Invariant(Constraints != null);
            Contract.Invariant(Variables != null);
            Contract.Invariant(Domains != null);
        }
    }
}
