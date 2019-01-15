using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core;
using Workbench.Core.Models;
using Workbench.Messages;
using Workbench.Properties;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Bundle editor view model.
    /// </summary>
    public sealed class BundleEditorViewModel : Conductor<ModelItemViewModel>.Collection.OneActive
    {
        private ICommand _addSingletonVariableCommand;
        private ICommand _addAggregateVariableCommand;
        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private ICommand _addDomainCommand;
        private ICommand _addExpressionConstraintCommand;
        private ICommand _addAllDifferentConstraintCommand;
        private ICommand _editCommand;
        private ICommand _addBucketCommand;
        private ICommand _addBundleCommand;
        private ICommand _deleteCommand;
        private ICommand _editNameCommand;

        /// <summary>
        /// Initialize a bundle editor view model with the bundle model, data service, window manager and event aggregator.
        /// </summary>
        public BundleEditorViewModel(BundleModel theBundle, IDataService theDataService, IWindowManager theWindowManager, IEventAggregator theEventAggregator)
        {
            Contract.Requires<ArgumentNullException>(theBundle != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);

            _windowManager = theWindowManager;
            _eventAggregator = theEventAggregator;
            Bundle = theBundle;
            DisplayName = GetBundleName();
            Variables = new BindableCollection<VariableModelItemViewModel>();
            Domains = new BindableCollection<SharedDomainModelItemViewModel>();
            Constraints = new BindableCollection<ConstraintModelItemViewModel>();
            Bundles = new BindableCollection<BundleModelItemViewModel>();
            AddSingletonVariableCommand = new CommandHandler(AddSingletonVariableAction);
            AddAggregateVariableCommand = new CommandHandler(AddAggregateVariableAction);
            AddDomainCommand = new CommandHandler(AddDomainAction);
            AddAllDifferentConstraintCommand = new CommandHandler(AddAllDifferentConstraintAction);
            AddExpressionConstraintCommand = new CommandHandler(AddExpressionConstraintAction);
            AddBucketCommand = new CommandHandler(AddBucketAction);
            AddBundleCommand = new CommandHandler(AddBundleAction);
            EditCommand = new CommandHandler(EditAction, IsItemSelected);
            DeleteCommand = new CommandHandler(DeleteAction, IsItemSelected);
            EditNameCommand = new CommandHandler(EditNameAction, x => !IsModel);
        }

        /// <summary>
        /// Gets whether the bundle is the root model.
        /// </summary>
        public bool IsModel => Bundle is ModelModel;

        /// <summary>
        /// Gets the bundle model currently being edited.
        /// </summary>
        public BundleModel Bundle { get; }

        /// <summary>
        /// Gets the collection of variables.
        /// </summary>
        public IObservableCollection<VariableModelItemViewModel> Variables { get; }

        /// <summary>
        /// Gets the collection of domains.
        /// </summary>
        public IObservableCollection<SharedDomainModelItemViewModel> Domains { get; }

        /// <summary>
        /// Gets the collection of constraints.
        /// </summary>
        public IObservableCollection<ConstraintModelItemViewModel> Constraints { get; }

        /// <summary>
        /// Gets all of the sub-bundles.
        /// </summary>
        public IObservableCollection<BundleModelItemViewModel> Bundles { get; }

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

        /// <summary>
        /// Gets or sets the Add Bucket command.
        /// </summary>
        public ICommand AddBucketCommand
        {
            get => _addBucketCommand;
            set
            {
                _addBucketCommand = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the Add Bundle command.
        /// </summary>
        public ICommand AddBundleCommand
        {
            get => _addBundleCommand;
            set
            {
                _addBundleCommand = value;
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
        public ICommand DeleteCommand
        {
            get => _deleteCommand;
            set
            {
                _deleteCommand = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the edit bundle name command.
        /// </summary>
        public ICommand EditNameCommand
        {
            get => _editNameCommand;
            set
            {
                _editNameCommand = value; 
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

            var newSingletonVariableItem = new SingletonVariableModelItemViewModel(newVariableModel, _windowManager, _eventAggregator);
            FixupSingletonVariable(newSingletonVariableItem);
            Bundle.AddVariable(newVariableModel);
        }

        /// <summary>
        /// Add a new aggregate variable to the model.
        /// </summary>
        /// <param name="newVariableModel">New variable.</param>
        public void AddAggregateVariable(AggregateVariableModel newVariableModel)
        {
            Contract.Requires<ArgumentNullException>(newVariableModel != null);

            var newAggregateVariableItem = new AggregateVariableModelItemViewModel(newVariableModel, _windowManager, _eventAggregator);
            FixupAggregateVariable(newAggregateVariableItem);
            Bundle.AddVariable(newVariableModel);
        }

        /// <summary>
        /// Add a new domain to the model.
        /// </summary>
        /// <param name="newDomainModel">New domain.</param>
        public void AddDomain(SharedDomainModel newDomainModel)
        {
            Contract.Requires<ArgumentNullException>(newDomainModel != null);

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
        /// Add a new bundle to the current bundle.
        /// </summary>
        /// <param name="newBundleModel">New bundle.</param>
        public void AddBundle(BundleModel newBundleModel)
        {
            var newBundleItem = new BundleModelItemViewModel(newBundleModel, Parent as ModelEditorTabViewModel);
            FixupBundle(newBundleItem);
            AddBundleToModel(newBundleItem);
            _eventAggregator.PublishOnUIThread(new BundleAddedMessage(newBundleModel));
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
            _eventAggregator.PublishOnUIThread(new VariableDeletedMessage(variableToDelete.Variable));
        }

        /// <summary>
        /// Delete the domain.
        /// </summary>
        /// <param name="domainToDelete">Domain to delete.</param>
        public void DeleteDomain(SharedDomainModelItemViewModel domainToDelete)
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
        /// Delete the bundle.
        /// </summary>
        /// <param name="bundleToDelete">Bundle to delete.</param>
        public void DeleteBundle(BundleModelItemViewModel bundleToDelete)
        {
            Contract.Requires<ArgumentNullException>(bundleToDelete != null);

            Bundles.Remove(bundleToDelete);
            DeactivateItem(bundleToDelete, close: true);
            DeleteBundleFromModel(bundleToDelete);
            _eventAggregator.PublishOnUIThread(new BundleDeletedMessage(bundleToDelete.Bundle));
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
        internal void FixupDomain(SharedDomainModelItemViewModel domainViewModel)
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
        /// Fixes up a bundle view model into the model.
        /// </summary>
        /// <param name="bundleItem">New bundle model item.</param>
        internal void FixupBundle(BundleModelItemViewModel bundleItem)
        {
            Contract.Requires<ArgumentNullException>(bundleItem != null);
            Bundles.Add(bundleItem);
            ActivateItem(bundleItem);
        }

        /// <summary>
        /// Add a new domain to the model model.
        /// </summary>
        /// <param name="newDomainModel">New domain view model.</param>
        private void AddDomainToModel(SharedDomainModel newDomainModel)
        {
            Contract.Assert(newDomainModel != null);

            newDomainModel.AssignIdentity();
            Bundle.AddSharedDomain(newDomainModel);
        }

        /// <summary>
        /// Add a new all different constraint to the model model.
        /// </summary>
        /// <param name="newConstraintViewModel">New constraint view model.</param>
        private void AddConstraintToModel(AllDifferentConstraintModelItemViewModel newConstraintViewModel)
        {
            Contract.Assert(newConstraintViewModel.Constraint != null);
            Bundle.AddConstraint(newConstraintViewModel.Constraint);
        }

        /// <summary>
        /// Add a new expression constraint to the model model.
        /// </summary>
        /// <param name="newConstraintViewModel">New constraint view model.</param>
        private void AddConstraintToModel(ExpressionConstraintModelItemViewModel newConstraintViewModel)
        {
            Contract.Assert(newConstraintViewModel.Model != null);
            newConstraintViewModel.Model.AssignIdentity();
            Bundle.AddConstraint(newConstraintViewModel.ExpressionConstraint);
        }

        private void DeleteConstraintFromModel(ConstraintModelItemViewModel constraintToDelete)
        {
            Contract.Assert(constraintToDelete.Model != null);
            Bundle.DeleteConstraint(constraintToDelete.Constraint);
        }

        private void DeleteVariableFromModel(VariableModelItemViewModel variableToDelete)
        {
            Contract.Assert(variableToDelete.Model != null);
            Bundle.DeleteVariable(variableToDelete.Variable);
        }

        private void DeleteDomainFromModel(SharedDomainModelItemViewModel domainToDelete)
        {
            Contract.Assert(domainToDelete.Model != null);
            Bundle.DeleteDomain(domainToDelete.Domain);
        }

        private void DeleteBundleFromModel(BundleModelItemViewModel bundleToDelete)
        {
            Contract.Assert(bundleToDelete.Model != null);
            Bundle.DeleteBundle(bundleToDelete.Bundle);
        }

        private void AddSingletonVariableAction()
        {
            var singletonVariableEditorViewModel = new SingletonVariableEditorViewModel();
            var x = _windowManager.ShowDialog(singletonVariableEditorViewModel);
            if (!x.GetValueOrDefault()) return;
            AddSingletonVariable(new SingletonVariableBuilder().WithName(singletonVariableEditorViewModel.VariableName)
                                                               .WithDomain(singletonVariableEditorViewModel.DomainExpression)
                                                               .Inside(Bundle)
                                                               .Build());
        }

        private void AddAggregateVariableAction()
        {
            var aggregateVariableEditorViewModel = new AggregateVariableEditorViewModel();
            var x = _windowManager.ShowDialog(aggregateVariableEditorViewModel);
            if (!x.GetValueOrDefault()) return;
            AddAggregateVariable(new AggregateVariableBuilder().WithName(aggregateVariableEditorViewModel.VariableName)
                                                               .WithDomain(aggregateVariableEditorViewModel.DomainExpression)
                                                               .WithSize(aggregateVariableEditorViewModel.Size)
                                                               .Inside(Bundle)
                                                               .Build());
        }

        private void AddDomainAction()
        {
            var domainEditorViewModel = new SharedDomainEditorViewModel();
            var x = _windowManager.ShowDialog(domainEditorViewModel);
            if (!x.GetValueOrDefault()) return;
            Bundle.AddSharedDomain(new SharedDomainBuilder().WithName(domainEditorViewModel.DomainName)
                                                         .Inside(Bundle)
                                                         .WithDomain(domainEditorViewModel.DomainExpression)
                                                         .Build());
        }

        private void AddExpressionConstraintAction()
        {
            var expressionConstraintEditViewModel = new ExpressionConstraintEditorViewModel();
            var x = _windowManager.ShowDialog(expressionConstraintEditViewModel);
            if (!x.GetValueOrDefault()) return;
            AddConstraint(new ExpressionConstraintBuilder().WithName(expressionConstraintEditViewModel.ConstraintName)
                                                           .Inside(Bundle)
                                                           .WithExpression(expressionConstraintEditViewModel.ConstraintExpression)
                                                           .Build());
        }

        private void AddAllDifferentConstraintAction()
        {
            var allDifferentConstraintEditViewModel = new AllDifferentConstraintEditorViewModel(Bundle);
            var result = _windowManager.ShowDialog(allDifferentConstraintEditViewModel);
            if (!result.GetValueOrDefault()) return;
            AddConstraint(new AllDifferentConstraintBuilder().WithName(allDifferentConstraintEditViewModel.ConstraintName)
                                                             .WithExpression(allDifferentConstraintEditViewModel.SelectedVariable)
                                                             .Inside(Bundle)
                                                             .Build());
        }

        private void AddBundleAction()
        {
            var bundleNameEditorViewModel = new BundleNameEditorViewModel();
            var result = _windowManager.ShowDialog(bundleNameEditorViewModel);
            if (!result.GetValueOrDefault()) return;
            AddBundle(new BundleModel(new ModelName(bundleNameEditorViewModel.BundleName)));
        }

        private void AddBucketAction()
        {
            throw new NotImplementedException();
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

                case SharedDomainModelItemViewModel domainItem:
                    DeleteDomain(domainItem);
                    break;

                case BundleModelItemViewModel bundleItem:
                    DeleteBundle(bundleItem);
                    break;

                default:
                    throw new NotImplementedException("Unknown item.");
            }
        }

        private void EditNameAction()
        {
            var oldName = Bundle.Name.Text;
            var bundleNameEditorViewModel = new BundleNameEditorViewModel();
            bundleNameEditorViewModel.BundleName = Bundle.Name.Text;
            var result = _windowManager.ShowDialog(bundleNameEditorViewModel);
            if (!result.GetValueOrDefault()) return;
            if (oldName == bundleNameEditorViewModel.BundleName) return;
            Bundle.Name.Text = DisplayName = bundleNameEditorViewModel.BundleName;
            _eventAggregator.PublishOnUIThread(new BundleRenamedMessage(oldName, Bundle));
        }

        private void AddBundleToModel(BundleModelItemViewModel newBundleItem)
        {
            Contract.Assert(newBundleItem.Bundle != null);
            Bundle.AddBundle(newBundleItem.Bundle);
        }

        private bool IsItemSelected(object parameter)
        {
            return ActiveItem != null;
        }

        private string GetBundleName()
        {
            if (IsModel) return Resources.ModelRootName;
            return Bundle.Name;
        }

        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(Bundle != null);
            Contract.Invariant(Constraints != null);
            Contract.Invariant(Variables != null);
            Contract.Invariant(Domains != null);
        }
    }
}