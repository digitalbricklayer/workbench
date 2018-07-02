using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Messages;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// A view model for a model.
    /// </summary>
    public sealed class ModelEditorTabViewModel : Conductor<GraphicViewModel>.Collection.AllActive
    {
        private readonly IWindowManager windowManager;
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Initialize a model view model with a model and window manager.
        /// </summary>
        public ModelEditorTabViewModel(IDataService theService,
                                       IWindowManager theWindowManager,
                                       IEventAggregator theEventAggregator)
        {
            Contract.Requires<ArgumentNullException>(theService != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);

            DisplayName = "Model";
            this.Model = theService.GetWorkspace().Model;
            this.windowManager = theWindowManager;
            this.eventAggregator = theEventAggregator;
            this.Variables = new BindableCollection<VariableEditorViewModel>();
            this.Domains = new BindableCollection<DomainEditorViewModel>();
            this.Constraints = new BindableCollection<ConstraintEditorViewModel>();
        }

        /// <summary>
        /// Gets the collection of variables in the model.
        /// </summary>
        public IObservableCollection<VariableEditorViewModel> Variables { get; private set; }

        /// <summary>
        /// Gets the collection of domains in the model.
        /// </summary>
        public IObservableCollection<DomainEditorViewModel> Domains { get; private set; }

        /// <summary>
        /// Gets the collection of constraints in the model.
        /// </summary>
        public IObservableCollection<ConstraintEditorViewModel> Constraints { get; private set; }

        /// <summary>
        /// Gets or sets the model model.
        /// </summary>
        public ModelModel Model { get; set; }

        /// <summary>
        /// Add a new singleton variable to the model.
        /// </summary>
        /// <param name="newVariableEditorViewModel">New variable.</param>
        public void AddSingletonVariable(SingletonVariableEditorViewModel newVariableEditorViewModel)
        {
            if (newVariableEditorViewModel == null)
                throw new ArgumentNullException("newVariableEditorViewModel");

            this.FixupSingletonVariable(newVariableEditorViewModel);
            this.AddSingletonVariableToModel(newVariableEditorViewModel);
            this.eventAggregator.PublishOnUIThread(new SingletonVariableAddedMessage(newVariableEditorViewModel));
        }

        /// <summary>
        /// Add a new aggregate variable to the model.
        /// </summary>
        /// <param name="newVariableEditorViewModel">New variable.</param>
        public void AddAggregateVariable(AggregateVariableEditorViewModel newVariableEditorViewModel)
        {
            if (newVariableEditorViewModel == null)
                throw new ArgumentNullException("newVariableEditorViewModel");

            this.FixupAggregateVariable(newVariableEditorViewModel);
            this.AddAggregateVariableToModel(newVariableEditorViewModel);
            this.eventAggregator.PublishOnUIThread(new AggregateVariableAddedMessage(newVariableEditorViewModel));
        }

        /// <summary>
        /// Add a new domain to the model.
        /// </summary>
        /// <param name="newDomainEditorViewModel">New domain.</param>
        public void AddDomain(DomainEditorViewModel newDomainEditorViewModel)
        {
            if (newDomainEditorViewModel == null)
                throw new ArgumentNullException("newDomainEditorViewModel");
            this.FixupDomain(newDomainEditorViewModel);
            this.AddDomainToModel(newDomainEditorViewModel);
        }

        /// <summary>
        /// Add a new constraint to the model.
        /// </summary>
        /// <param name="newConstraintEditorViewModel">New constraint.</param>
        public void AddConstraint(ConstraintEditorViewModel newConstraintEditorViewModel)
        {
            if (newConstraintEditorViewModel == null)
                throw new ArgumentNullException("newConstraintEditorViewModel");
            this.FixupConstraint(newConstraintEditorViewModel);
            this.AddConstraintToModel(newConstraintEditorViewModel);
        }

        /// <summary>
        /// Delete the variable.
        /// </summary>
        /// <param name="variableToDelete">Variable to delete.</param>
        public void DeleteVariable(VariableEditorViewModel variableToDelete)
        {
            if (variableToDelete == null)
                throw new ArgumentNullException("variableToDelete");
            this.Variables.Remove(variableToDelete);
            this.DeactivateItem(variableToDelete, close: true);
            this.DeleteVariableFromModel(variableToDelete);
            this.eventAggregator.PublishOnUIThread(new VariableDeletedMessage(variableToDelete));
        }

        /// <summary>
        /// Delete the domain.
        /// </summary>
        /// <param name="domainToDelete">Domain to delete.</param>
        public void DeleteDomain(DomainEditorViewModel domainToDelete)
        {
            if (domainToDelete == null)
                throw new ArgumentNullException("domainToDelete");
            this.Domains.Remove(domainToDelete);
            this.DeactivateItem(domainToDelete, close: true);
            this.DeleteDomainFromModel(domainToDelete);
        }

        /// <summary>
        /// Delete the constraint.
        /// </summary>
        /// <param name="constraintToDelete">Constraint to delete.</param>
        public void DeleteConstraint(ConstraintEditorViewModel constraintToDelete)
        {
            if (constraintToDelete == null)
                throw new ArgumentNullException("constraintToDelete");
            this.Constraints.Remove(constraintToDelete);
            this.DeactivateItem(constraintToDelete, close: true);
            this.DeleteConstraintFromModel(constraintToDelete);
        }

        /// <summary>
        /// Solve the model.
        /// </summary>
        public bool Validate()
        {
            var validationContext = new ModelValidationContext();
            var isModelValid = Model.Validate(validationContext);
            if (isModelValid) return true;
            Contract.Assume(validationContext.HasErrors);
            DisplayErrorDialog(validationContext);

            return false;
        }

        /// <summary>
        /// Get the singleton variable matching the given name.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <returns>Variable matching the name.</returns>
        public VariableEditorViewModel GetVariableByName(string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
                throw new ArgumentNullException("variableName");
            return this.Variables.FirstOrDefault(_ => _.Name == variableName);
        }

        /// <summary>
        /// Get the constraint with the constraint name.
        /// </summary>
        /// <param name="constraintName">Name of the constraint.</param>
        /// <returns>Constraint view model matching the name.</returns>
        public ConstraintEditorViewModel GetConstraintByName(string constraintName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(constraintName));
            return this.Constraints.FirstOrDefault(_ => _.Name == constraintName);
        }

        /// <summary>
        /// Get all selected aggregate variables.
        /// </summary>
        /// <returns>All selected variables.</returns>
        public IList<VariableEditorViewModel> GetSelectedAggregateVariables()
        {
            return Variables.Where(_ => _.IsSelected && _.IsAggregate)
                            .ToList();
        }

        /// <summary>
        /// Reset the contents of the model.
        /// </summary>
        public void Reset()
        {
            this.Variables.Clear();
            this.Constraints.Clear();
            this.Domains.Clear();
            this.Items.Clear();
        }

        /// <summary>
        /// Fixes up a singleton variable view model into the model.
        /// </summary>
        /// <remarks>
        /// Used when mapping the model to a view model.
        /// </remarks>
        /// <param name="VariableEditorViewModel">Singleton variable view model.</param>
        internal void FixupSingletonVariable(VariableEditorViewModel VariableEditorViewModel)
        {
            if (VariableEditorViewModel == null)
                throw new ArgumentNullException(nameof(VariableEditorViewModel));
            this.ActivateItem(VariableEditorViewModel);
            this.Variables.Add(VariableEditorViewModel);
        }

        /// <summary>
        /// Fixes up an aggregate variable view model into the model.
        /// </summary>
        /// <remarks>
        /// Used when mapping the model to a view model.
        /// </remarks>
        /// <param name="VariableEditorViewModel">Aggregate variable view model.</param>
        internal void FixupAggregateVariable(AggregateVariableEditorViewModel VariableEditorViewModel)
        {
            if (VariableEditorViewModel == null)
                throw new ArgumentNullException("VariableEditorViewModel");
            this.ActivateItem(VariableEditorViewModel);
            this.Variables.Add(VariableEditorViewModel);
        }

        /// <summary>
        /// Fixes up a domain view model into the model.
        /// </summary>
        /// <remarks>
        /// Used when mapping the model to a view model.
        /// </remarks>
        /// <param name="DomainEditorViewModel">Domain view model.</param>
        internal void FixupDomain(DomainEditorViewModel DomainEditorViewModel)
        {
            if (DomainEditorViewModel == null)
                throw new ArgumentNullException("DomainEditorViewModel");
            this.ActivateItem(DomainEditorViewModel);
            this.Domains.Add(DomainEditorViewModel);
        }

        /// <summary>
        /// Fixes up a constraint view model into the model.
        /// </summary>
        /// <remarks>
        /// Used when mapping the model to a view model.
        /// </remarks>
        /// <param name="ConstraintEditorViewModel">Constraint view model.</param>
        internal void FixupConstraint(ConstraintEditorViewModel ConstraintEditorViewModel)
        {
            if (ConstraintEditorViewModel == null)
                throw new ArgumentNullException("ConstraintEditorViewModel");
            this.ActivateItem(ConstraintEditorViewModel);
            this.Constraints.Add(ConstraintEditorViewModel);
        }

        /// <summary>
        /// Add a new variable to the model model.
        /// </summary>
        /// <param name="newVariableEditorViewModel">New variable view model.</param>
        private void AddSingletonVariableToModel(SingletonVariableEditorViewModel newVariableEditorViewModel)
        {
            Debug.Assert(newVariableEditorViewModel.Model != null);
            this.Model.AddVariable(newVariableEditorViewModel.SingletonVariableGraphic.SingletonVariable);
        }

        /// <summary>
        /// Add a new variable to the model model.
        /// </summary>
        /// <param name="newVariableEditorViewModel">New variable view model.</param>
        private void AddAggregateVariableToModel(AggregateVariableEditorViewModel newVariableEditorViewModel)
        {
            Debug.Assert(newVariableEditorViewModel.Model != null);
            this.Model.AddVariable(newVariableEditorViewModel.AggregateVariableGraphic.AggregateVariable);
        }

        /// <summary>
        /// Add a new domain to the model model.
        /// </summary>
        /// <param name="newDomainEditorViewModel">New domain view model.</param>
        private void AddDomainToModel(DomainEditorViewModel newDomainEditorViewModel)
        {
            Debug.Assert(newDomainEditorViewModel.Model != null);
            this.Model.AddDomain(newDomainEditorViewModel.DomainGraphic.Domain);
        }

        /// <summary>
        /// Add a new constraint to the model model.
        /// </summary>
        /// <param name="newConstraintEditorViewModel">New constraint view model.</param>
        private void AddConstraintToModel(ConstraintEditorViewModel newConstraintEditorViewModel)
        {
            Debug.Assert(newConstraintEditorViewModel.Model != null);
            this.Model.AddConstraint(newConstraintEditorViewModel.ConstraintGraphic.Constraint);
        }

        private void DeleteConstraintFromModel(ConstraintEditorViewModel constraintToDelete)
        {
            Debug.Assert(constraintToDelete.Model != null);
            this.Model.DeleteConstraint(constraintToDelete.ConstraintGraphic.Constraint);
        }

        private void DeleteVariableFromModel(VariableEditorViewModel variableToDelete)
        {
            Debug.Assert(variableToDelete.Model != null);
            this.Model.DeleteVariable(variableToDelete.VariableGraphic.Variable);
        }

        private void DeleteDomainFromModel(DomainEditorViewModel domainToDelete)
        {
            Debug.Assert(domainToDelete.Model != null);
            this.Model.DeleteDomain(domainToDelete.DomainGraphic.Domain);
        }

        /// <summary>
        /// Display a dialog box with a display of all of the model errors.
        /// </summary>
        /// <param name="theContext">Validation context.</param>
        private void DisplayErrorDialog(ModelValidationContext theContext)
        {
            var errorsViewModel = CreateModelErrorsFrom(theContext);
            this.windowManager.ShowDialog(errorsViewModel);
        }

        /// <summary>
        /// Create a model errros view model from a model.
        /// </summary>
        /// <param name="theContext">Validation context.</param>
        /// <returns>View model with all errors in the model.</returns>
        private static ModelErrorsViewModel CreateModelErrorsFrom(ModelValidationContext theContext)
        {
            Contract.Requires<ArgumentNullException>(theContext != null);
            Contract.Requires<InvalidOperationException>(theContext.HasErrors);

            var errorsViewModel = new ModelErrorsViewModel();
            foreach (var error in theContext.Errors)
            {
                var errorViewModel = new ModelErrorViewModel
                {
                    Message = error
                };
                errorsViewModel.Errors.Add(errorViewModel);
            }

            return errorsViewModel;
        }

        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(this.Model != null);
            Contract.Invariant(this.Constraints != null);
            Contract.Invariant(this.Variables != null);
            Contract.Invariant(this.Domains != null);
        }
    }
}