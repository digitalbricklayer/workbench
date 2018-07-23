using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the model editor.
    /// </summary>
    public sealed class ModelEditorTabViewModel : Conductor<ItemViewModel>.Collection.AllActive, ITabViewModel
    {
        private ICommand _addSingletonVariableCommand;
        private ICommand _addAggregateVariableCommand;
        private readonly IAppRuntime _appRuntime;
        private readonly IWindowManager _windowManager;
        private ICommand _addDomainCommand;
        private ICommand _addExpressionConstraintCommand;
        private ICommand _addAllDifferentConstraintCommand;

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
            Variables = new BindableCollection<VariableItemViewModel>();
            Domains = new BindableCollection<DomainItemViewModel>();
            Constraints = new BindableCollection<ConstraintItemViewModel>();
            AddSingletonVariableCommand =  new CommandHandler(AddSingletonVariableAction);
            AddAggregateVariableCommand = new CommandHandler(AddAggregateVariableAction);
            AddDomainCommand = new CommandHandler(AddDomainAction);
            AddAllDifferentConstraintCommand = new CommandHandler(AddAllDifferentConstraintAction);
            AddExpressionConstraintCommand = new CommandHandler(AddExpressionConstraintAction);
        }

        /// <summary>
        /// Gets the work area view model.
        /// </summary>
        public WorkAreaViewModel WorkArea
        {
            get { return _appRuntime.WorkArea; }
        }

        /// <summary>
        /// Gets or sets the model model.
        /// </summary>
        public ModelModel ModelModel { get; set; }

        /// <summary>
        /// Gets the collection of variables.
        /// </summary>
        public IObservableCollection<VariableItemViewModel> Variables { get; private set; }

        /// <summary>
        /// Gets the collection of domains.
        /// </summary>
        public IObservableCollection<DomainItemViewModel> Domains { get; private set; }

        /// <summary>
        /// Gets the collection of constraints.
        /// </summary>
        public IObservableCollection<ConstraintItemViewModel> Constraints { get; private set; }

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

        /// <summary>
        /// Add a new singleton variable.
        /// </summary>
        /// <param name="newVariableModel">New variable.</param>
        public void AddSingletonVariable(SingletonVariableModel newVariableModel)
        {
            Contract.Requires<ArgumentNullException>(newVariableModel != null);

            var newSingletonVariableItem = new SingletonVariableItemViewModel(newVariableModel);
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

            var newAggregateVariableItem = new AggregateVariableItemViewModel(newVariableModel);
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

            var newDomainItem = new DomainItemViewModel(newDomainModel);
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

            var newConstraint = new AllDifferentConstraintItemViewModel(newAllDifferentConstraint);
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

            var newExpressionConstraintEditor = new ExpressionConstraintItemViewModel(newExpressionConstraint);
            FixupConstraint(newExpressionConstraintEditor);
            AddConstraintToModel(newExpressionConstraintEditor);
        }

        /// <summary>
        /// Delete the variable.
        /// </summary>
        /// <param name="variableToDelete">Variable to delete.</param>
        public void DeleteVariable(VariableModel variableToDelete)
        {
            Contract.Requires<ArgumentNullException>(variableToDelete != null);

#if false
            Items.Remove(variableToDelete);
            Variables.Remove(variableToDelete);
            DeactivateItem(variableToDelete, close: true);
            DeleteVariableFromModel(variableToDelete);
            
#endif
        }

        /// <summary>
        /// Delete the domain.
        /// </summary>
        /// <param name="domainToDelete">Domain to delete.</param>
        public void DeleteDomain(DomainItemViewModel domainToDelete)
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
        public void DeleteConstraint(ConstraintItemViewModel constraintToDelete)
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
        internal void FixupSingletonVariable(SingletonVariableItemViewModel variableViewModel)
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
        internal void FixupAggregateVariable(AggregateVariableItemViewModel variableViewModel)
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
        internal void FixupDomain(DomainItemViewModel domainViewModel)
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
        internal void FixupConstraint(ConstraintItemViewModel constraintViewModel)
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
        private void AddConstraintToModel(AllDifferentConstraintItemViewModel newConstraintViewModel)
        {
            Contract.Assert(newConstraintViewModel.Constraint != null);
            ModelModel.AddConstraint(newConstraintViewModel.Constraint);
        }

        /// <summary>
        /// Add a new expression constraint to the model model.
        /// </summary>
        /// <param name="newConstraintViewModel">New constraint view model.</param>
        private void AddConstraintToModel(ExpressionConstraintItemViewModel newConstraintViewModel)
        {
            Contract.Assert(newConstraintViewModel.Model != null);
            newConstraintViewModel.Model.AssignIdentity();
            ModelModel.AddConstraint(newConstraintViewModel.ExpressionConstraint);
        }

        private void DeleteConstraintFromModel(ConstraintItemViewModel constraintToDelete)
        {
            Contract.Assert(constraintToDelete.Model != null);
            ModelModel.DeleteConstraint(constraintToDelete.Constraint);
        }

        private void DeleteVariableFromModel(VariableItemViewModel variableToDelete)
        {
            Contract.Assert(variableToDelete.Model != null);
            ModelModel.DeleteVariable(variableToDelete.Variable);
        }

        private void DeleteDomainFromModel(DomainItemViewModel domainToDelete)
        {
            Contract.Assert(domainToDelete.Model != null);
            ModelModel.DeleteDomain(domainToDelete.Domain);
        }

        private void AddSingletonVariableAction()
        {
            var singletonVariableEditorViewModel = new SingletonVariableEditViewModel();
            var x = _windowManager.ShowDialog(singletonVariableEditorViewModel);
            if (!x.HasValue) return;
            WorkArea.AddSingletonVariable(new SingletonVariableBuilder().WithName(singletonVariableEditorViewModel.VariableName)
                                                                        .WithDomain(singletonVariableEditorViewModel.DomainExpression)
                                                                        .WithModel(WorkArea.WorkspaceModel.Model)
                                                                        .Build());
        }

        private void AddAggregateVariableAction()
        {
            var aggregateVariableEditorViewModel = new AggregateVariableEditViewModel();
            var x = _windowManager.ShowDialog(aggregateVariableEditorViewModel);
            if (!x.HasValue) return;
            WorkArea.AddAggregateVariable(new AggregateVariableBuilder().WithName(aggregateVariableEditorViewModel.VariableName)
                                                                        .WithDomain(aggregateVariableEditorViewModel.DomainExpression)
                                                                        .WithSize(aggregateVariableEditorViewModel.Size)
                                                                        .WithModel(WorkArea.WorkspaceModel.Model)
                                                                        .Build());
        }

        private void AddDomainAction()
        {
            var domainEditorViewModel = new DomainEditViewModel();
            var x = _windowManager.ShowDialog(domainEditorViewModel);
            if (!x.HasValue) return;
            WorkArea.AddDomain(new DomainBuilder().WithName(domainEditorViewModel.DomainName)
                                                  .WithDomain(domainEditorViewModel.DomainExpression)
                                                  .Build());
        }

        private void AddExpressionConstraintAction()
        {
            var expressionConstraintEditViewModel = new ExpressionConstraintEditViewModel();
            var x = _windowManager.ShowDialog(expressionConstraintEditViewModel);
            if (!x.HasValue) return;
            WorkArea.AddExpressionConstraint(new ExpressionConstraintBuilder().WithName(expressionConstraintEditViewModel.ConstraintName)
                                                                              .WithExpression(expressionConstraintEditViewModel.ConstraintExpression)
                                                                              .Build());
        }

        private void AddAllDifferentConstraintAction()
        {
            var allDifferentConstraintEditViewModel = new AllDifferentConstraintEditViewModel();
            var x = _windowManager.ShowDialog(allDifferentConstraintEditViewModel);
            if (!x.HasValue) return;
            WorkArea.AddAllDifferentConstraint(new AllDifferentConstraintBuilder().WithName(allDifferentConstraintEditViewModel.ConstraintName)
                                                                                  .WithExpression(allDifferentConstraintEditViewModel.ConstraintExpression)
                                                                                  .Build());
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
