using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core;
using Workbench.Core.Models;
using Workbench.Messages;
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
        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private ICommand _addDomainCommand;
        private ICommand _addExpressionConstraintCommand;
        private ICommand _addAllDifferentConstraintCommand;
        private ICommand _editCommand;
        private CommandHandler _deleteCommand;
        private readonly IShell _shell;

        /// <summary>
        /// Initialize a solution designer view model with default values.
        /// </summary>
        public ModelEditorTabViewModel(IShell theShell,
                                       IDataService theDataService,
                                       IWindowManager theWindowManager,
                                       IEventAggregator theEventAggregator)
        {
            _shell = theShell;
            _windowManager = theWindowManager;
            _eventAggregator = theEventAggregator;
            DisplayName = "Model";
            ModelModel = theDataService.GetWorkspace().Model;
            Variables = new BindableCollection<VariableModelItemViewModel>();
            Domains = new BindableCollection<SharedDomainModelItemViewModel>();
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
        /// Gets the workspace view model.
        /// </summary>
        public IWorkspace Workspace => _shell.Workspace;

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
        public IObservableCollection<SharedDomainModelItemViewModel> Domains { get; private set; }

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
        /// Gets whether the currently selected tab be closed by the user.
        /// </summary>
        public bool CloseTabIsVisible => false;

        /// <summary>
        /// Gets or sets the tab text.
        /// </summary>
        public string TabText
        {
            get => DisplayName;
            set
            {
                DisplayName = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Add a new singleton variable.
        /// </summary>
        /// <param name="newVariableModel">New variable.</param>
        public void AddSingletonVariable(SingletonVariableModel newVariableModel)
        {
            var newSingletonVariableItem = new SingletonVariableModelItemViewModel(newVariableModel, _windowManager, _eventAggregator);
            FixupSingletonVariable(newSingletonVariableItem);
            ModelModel.AddVariable(newVariableModel);
        }

        /// <summary>
        /// Add a new aggregate variable to the model.
        /// </summary>
        /// <param name="newVariableModel">New variable.</param>
        public void AddAggregateVariable(AggregateVariableModel newVariableModel)
        {
            var newAggregateVariableItem = new AggregateVariableModelItemViewModel(newVariableModel, _windowManager, _eventAggregator);
            FixupAggregateVariable(newAggregateVariableItem);
            ModelModel.AddVariable(newVariableModel);
        }

        /// <summary>
        /// Add a new domain to the model.
        /// </summary>
        /// <param name="newDomainModel">New domain.</param>
        public void AddDomain(SharedDomainModel newDomainModel)
        {
            var newDomainItem = new SharedDomainModelItemViewModel(newDomainModel, _windowManager);
            FixupDomain(newDomainItem);
            AddDomainToModel(newDomainModel);
        }

        /// <summary>
        /// Add a new all different constraint to the model.
        /// </summary>
        /// <param name="newAllDifferentConstraint">New constraint.</param>
        public void AddConstraint(AllDifferentConstraintModel newAllDifferentConstraint)
        {
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
            Items.Remove(variableToDelete);
            Variables.Remove(variableToDelete);
            DeactivateItem(variableToDelete, close: true);
            DeleteVariableFromModel(variableToDelete);
            _eventAggregator.PublishOnUIThread(new VariableDeletedMessage(variableToDelete.Variable));
        }

        /// <summary>
        /// Delete the domain.
        /// </summary>
        /// <param name="domainToDelete">Domain to delete.</param>
        public void DeleteDomain(SharedDomainModelItemViewModel domainToDelete)
        {
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
            Constraints.Remove(constraintToDelete);
            DeactivateItem(constraintToDelete, close: true);
            DeleteConstraintFromModel(constraintToDelete);
        }

        /// <summary>
        /// Get the variable by name.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <returns>Variable matching the name or null if the variable doesn't exist.</returns>
        public VariableModelItemViewModel GetVariableByName(string variableName)
        {
            return Variables.FirstOrDefault(variableItem => variableItem.Variable.Name == variableName);
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
        internal void FixupDomain(SharedDomainModelItemViewModel domainViewModel)
        {
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
            Constraints.Add(constraintViewModel);
            ActivateItem(constraintViewModel);
        }

        /// <summary>
        /// Add a new domain to the model model.
        /// </summary>
        /// <param name="newDomainModel">New domain view model.</param>
        private void AddDomainToModel(SharedDomainModel newDomainModel)
        {
            Debug.Assert(newDomainModel != null);

            newDomainModel.AssignIdentity();
            ModelModel.AddSharedDomain(newDomainModel);
        }

        /// <summary>
        /// Add a new all different constraint to the model model.
        /// </summary>
        /// <param name="newConstraintViewModel">New constraint view model.</param>
        private void AddConstraintToModel(AllDifferentConstraintModelItemViewModel newConstraintViewModel)
        {
            Debug.Assert(newConstraintViewModel.Constraint != null);
            ModelModel.AddConstraint(newConstraintViewModel.Constraint);
        }

        /// <summary>
        /// Add a new expression constraint to the model model.
        /// </summary>
        /// <param name="newConstraintViewModel">New constraint view model.</param>
        private void AddConstraintToModel(ExpressionConstraintModelItemViewModel newConstraintViewModel)
        {
            Debug.Assert(newConstraintViewModel.Model != null);
            newConstraintViewModel.Model.AssignIdentity();
            ModelModel.AddConstraint(newConstraintViewModel.ExpressionConstraint);
        }

        private void DeleteConstraintFromModel(ConstraintModelItemViewModel constraintToDelete)
        {
            Debug.Assert(constraintToDelete.Model != null);
            ModelModel.DeleteConstraint(constraintToDelete.Constraint);
        }

        private void DeleteVariableFromModel(VariableModelItemViewModel variableToDelete)
        {
            Debug.Assert(variableToDelete.Model != null);
            ModelModel.DeleteVariable(variableToDelete.Variable);
        }

        private void DeleteDomainFromModel(SharedDomainModelItemViewModel domainToDelete)
        {
            Debug.Assert(domainToDelete.Model != null);
            ModelModel.DeleteDomain(domainToDelete.Domain);
        }

        private void AddSingletonVariableAction()
        {
            var singletonVariableEditorViewModel = new SingletonVariableEditorViewModel();
            var x = _windowManager.ShowDialog(singletonVariableEditorViewModel);
            if (!x.GetValueOrDefault()) return;
            Workspace.AddSingletonVariable(new SingletonVariableBuilder().WithName(singletonVariableEditorViewModel.VariableName)
                                                                        .WithDomain(singletonVariableEditorViewModel.DomainExpression)
                                                                        .Inside(Workspace.WorkspaceModel.Model)
                                                                        .Build());
        }

        private void AddAggregateVariableAction()
        {
            var aggregateVariableEditorViewModel = new AggregateVariableEditorViewModel();
            var x = _windowManager.ShowDialog(aggregateVariableEditorViewModel);
            if (!x.GetValueOrDefault()) return;
            Workspace.AddAggregateVariable(new AggregateVariableBuilder().WithName(aggregateVariableEditorViewModel.VariableName)
                                                                        .WithDomain(aggregateVariableEditorViewModel.DomainExpression)
                                                                        .WithSize(aggregateVariableEditorViewModel.Size)
                                                                        .Inside(Workspace.WorkspaceModel.Model)
                                                                        .Build());
        }

        private void AddDomainAction()
        {
            var domainEditorViewModel = new SharedDomainEditorViewModel();
            var x = _windowManager.ShowDialog(domainEditorViewModel);
            if (!x.GetValueOrDefault()) return;
            Workspace.AddDomain(new SharedDomainBuilder().WithName(domainEditorViewModel.DomainName)
                                                         .Inside(Workspace.WorkspaceModel.Model)
                                                         .WithDomain(domainEditorViewModel.DomainExpression)
                                                         .Build());
        }

        private void AddExpressionConstraintAction()
        {
            var expressionConstraintEditViewModel = new ExpressionConstraintEditorViewModel();
            var x = _windowManager.ShowDialog(expressionConstraintEditViewModel);
            if (!x.GetValueOrDefault()) return;
            Workspace.AddExpressionConstraint(new ExpressionConstraintBuilder().WithName(expressionConstraintEditViewModel.ConstraintName)
                                                                               .Inside(Workspace.WorkspaceModel.Model)
                                                                               .WithExpression(expressionConstraintEditViewModel.ConstraintExpression)
                                                                               .Build());
        }

        private void AddAllDifferentConstraintAction()
        {
            var allDifferentConstraintEditViewModel = new AllDifferentConstraintEditorViewModel(Workspace.WorkspaceModel.Model);
            var result = _windowManager.ShowDialog(allDifferentConstraintEditViewModel);
            if (!result.GetValueOrDefault()) return;
            Workspace.AddAllDifferentConstraint(new AllDifferentConstraintBuilder().WithName(allDifferentConstraintEditViewModel.ConstraintName)
                                                                                   .WithExpression(allDifferentConstraintEditViewModel.SelectedVariable)
                                                                                   .Inside(Workspace.WorkspaceModel.Model)
                                                                                   .Build());
        }

        private void EditAction()
        {
            Debug.Assert(ActiveItem != null);
            ActiveItem.Edit();
        }

        private void DeleteAction()
        {
            Debug.Assert(ActiveItem != null);

            switch (ActiveItem)
            {
                case VariableModelItemViewModel variableItem:
                    DeleteVariable(variableItem);
                    break;

                case ConstraintModelItemViewModel constraintItem:
                    DeleteConstraint(constraintItem);
                    break;

                case SharedDomainModelItemViewModel domainItem:
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
    }
}
