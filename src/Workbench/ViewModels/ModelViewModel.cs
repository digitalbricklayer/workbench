using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Core.Solver;
using Workbench.Messages;

namespace Workbench.ViewModels
{
    /// <summary>
    /// A view model for a model.
    /// </summary>
    public sealed class ModelViewModel : Conductor<GraphicViewModel>.Collection.AllActive
    {
        private readonly IWindowManager windowManager;
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Initialize a model view model with a model and window manager.
        /// </summary>
        public ModelViewModel(ModelModel theModel, 
                              IWindowManager theWindowManager, 
                              IEventAggregator theEventAggregator)
        {
            if (theModel == null)
                throw new ArgumentNullException("theModel");

            if (theWindowManager == null)
                throw new ArgumentNullException("theWindowManager");

            if (theEventAggregator == null)
                throw new ArgumentNullException("theEventAggregator");

            this.Model = theModel;
            this.windowManager = theWindowManager;
            this.eventAggregator = theEventAggregator;
            this.Variables = new BindableCollection<VariableViewModel>();
            this.Domains = new BindableCollection<DomainViewModel>();
            this.Constraints = new BindableCollection<ConstraintViewModel>();
        }

        /// <summary>
        /// Gets the collection of variables in the model.
        /// </summary>
        public IObservableCollection<VariableViewModel> Variables { get; private set; }

        /// <summary>
        /// Gets the collection of domains in the model.
        /// </summary>
        public IObservableCollection<DomainViewModel> Domains { get; private set; }

        /// <summary>
        /// Gets the collection of constraints in the model.
        /// </summary>
        public IObservableCollection<ConstraintViewModel> Constraints { get; private set; }

        /// <summary>
        /// Gets or sets the model model.
        /// </summary>
        public ModelModel Model { get; set; }

        /// <summary>
        /// Add a new singleton variable to the model.
        /// </summary>
        /// <param name="newVariableViewModel">New variable.</param>
        public void AddSingletonVariable(VariableViewModel newVariableViewModel)
        {
            if (newVariableViewModel == null)
                throw new ArgumentNullException("newVariableViewModel");

            this.FixupSingletonVariable(newVariableViewModel);
            this.AddSingletonVariableToModel(newVariableViewModel);
            this.eventAggregator.PublishOnUIThread(new SingletonVariableAddedMessage(newVariableViewModel));
        }

        /// <summary>
        /// Add a new aggregate variable to the model.
        /// </summary>
        /// <param name="newVariableViewModel">New variable.</param>
        public void AddAggregateVariable(AggregateVariableViewModel newVariableViewModel)
        {
            if (newVariableViewModel == null)
                throw new ArgumentNullException("newVariableViewModel");

            this.FixupAggregateVariable(newVariableViewModel);
            this.AddAggregateVariableToModel(newVariableViewModel);
            this.eventAggregator.PublishOnUIThread(new AggregateVariableAddedMessage(newVariableViewModel));
        }

        /// <summary>
        /// Add a new domain to the model.
        /// </summary>
        /// <param name="newDomainViewModel">New domain.</param>
        public void AddDomain(DomainViewModel newDomainViewModel)
        {
            if (newDomainViewModel == null)
                throw new ArgumentNullException("newDomainViewModel");
            this.FixupDomain(newDomainViewModel);
            this.AddDomainToModel(newDomainViewModel);
        }

        /// <summary>
        /// Add a new constraint to the model.
        /// </summary>
        /// <param name="newConstraintViewModel">New constraint.</param>
        public void AddConstraint(ConstraintViewModel newConstraintViewModel)
        {
            if (newConstraintViewModel == null)
                throw new ArgumentNullException("newConstraintViewModel");
            this.FixupConstraint(newConstraintViewModel);
            this.AddConstraintToModel(newConstraintViewModel);
        }

        /// <summary>
        /// Delete the variable.
        /// </summary>
        /// <param name="variableToDelete">Variable to delete.</param>
        public void DeleteVariable(VariableViewModel variableToDelete)
        {
            if (variableToDelete == null) 
                throw new ArgumentNullException("variableToDelete");
            this.Variables.Remove(variableToDelete);
            this.DeactivateItem(variableToDelete, close:true);
            this.DeleteVariableFromModel(variableToDelete);
        }

        /// <summary>
        /// Delete the domain.
        /// </summary>
        /// <param name="domainToDelete">Domain to delete.</param>
        public void DeleteDomain(DomainViewModel domainToDelete)
        {
            if (domainToDelete == null) 
                throw new ArgumentNullException("domainToDelete");
            this.Domains.Remove(domainToDelete);
            this.DeactivateItem(domainToDelete, close:true);
            this.DeleteDomainFromModel(domainToDelete);
        }

        /// <summary>
        /// Delete the constraint.
        /// </summary>
        /// <param name="constraintToDelete">Constraint to delete.</param>
        public void DeleteConstraint(ConstraintViewModel constraintToDelete)
        {
            if (constraintToDelete == null)
                throw new ArgumentNullException("constraintToDelete");
            this.Constraints.Remove(constraintToDelete);
            this.DeactivateItem(constraintToDelete, close:true);
            this.DeleteConstraintFromModel(constraintToDelete);
        }

        /// <summary>
        /// Solve the model.
        /// </summary>
        public SolveResult Solve()
        {
            var theModel = this.Model;
            var isModelValid = theModel.Validate();
            if (!isModelValid)
            {
                Trace.Assert(theModel.Errors.Any());

                this.DisplayErrorDialog(theModel);
                return SolveResult.InvalidModel;
            }

            Trace.Assert(!theModel.Errors.Any());

            var solver = new OrToolsSolver();

            return solver.Solve(theModel);
        }

        /// <summary>
        /// Get the singleton variable matching the given name.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <returns>Variable matching the name.</returns>
        public VariableViewModel GetVariableByName(string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
                throw new ArgumentNullException("variableName");
            return this.Variables.FirstOrDefault(_ => _.Name == variableName);
        }

        /// <summary>
        /// Get all selected aggregate variables.
        /// </summary>
        /// <returns>All selected variables.</returns>
        public List<VariableViewModel> GetSelectedAggregateVariables()
        {
            return this.Variables.Where(_ => _.IsSelected && _.IsAggregate)
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
        /// <param name="variableViewModel">Singleton variable view model.</param>
        internal void FixupSingletonVariable(VariableViewModel variableViewModel)
        {
            if (variableViewModel == null)
                throw new ArgumentNullException("variableViewModel");
            this.ActivateItem(variableViewModel);
            this.Variables.Add(variableViewModel);
        }

        /// <summary>
        /// Fixes up an aggregate variable view model into the model.
        /// </summary>
        /// <remarks>
        /// Used when mapping the model to a view model.
        /// </remarks>
        /// <param name="variableViewModel">Aggregate variable view model.</param>
        internal void FixupAggregateVariable(AggregateVariableViewModel variableViewModel)
        {
            if (variableViewModel == null)
                throw new ArgumentNullException("variableViewModel");
            this.ActivateItem(variableViewModel);
            this.Variables.Add(variableViewModel);
        }

        /// <summary>
        /// Fixes up a domain view model into the model.
        /// </summary>
        /// <remarks>
        /// Used when mapping the model to a view model.
        /// </remarks>
        /// <param name="domainViewModel">Domain view model.</param>
        internal void FixupDomain(DomainViewModel domainViewModel)
        {
            if (domainViewModel == null)
                throw new ArgumentNullException("domainViewModel");
            this.ActivateItem(domainViewModel);
            this.Domains.Add(domainViewModel);
        }

        /// <summary>
        /// Fixes up a constraint view model into the model.
        /// </summary>
        /// <remarks>
        /// Used when mapping the model to a view model.
        /// </remarks>
        /// <param name="constraintViewModel">Constraint view model.</param>
        internal void FixupConstraint(ConstraintViewModel constraintViewModel)
        {
            if (constraintViewModel == null)
                throw new ArgumentNullException("constraintViewModel");
            this.ActivateItem(constraintViewModel);
            this.Constraints.Add(constraintViewModel);
        }

        /// <summary>
        /// Create a model errros view model from a model.
        /// </summary>
        /// <param name="aModel">Model with errors.</param>
        /// <returns>View model with all errors in the model.</returns>
        private static ModelErrorsViewModel CreateModelErrorsFrom(ModelModel aModel)
        {
            Trace.Assert(aModel.Errors.Any());

            var errorsViewModel = new ModelErrorsViewModel();
            foreach (var error in aModel.Errors)
            {
                var errorViewModel = new ModelErrorViewModel
                {
                    Message = error
                };
                errorsViewModel.Errors.Add(errorViewModel);
            }

            return errorsViewModel;
        }

        /// <summary>
        /// Add a new variable to the model model.
        /// </summary>
        /// <param name="newVariableViewModel">New variable view model.</param>
        private void AddSingletonVariableToModel(VariableViewModel newVariableViewModel)
        {
            Debug.Assert(newVariableViewModel.Model != null);
            this.Model.AddVariable(newVariableViewModel.Model);
        }

        /// <summary>
        /// Add a new variable to the model model.
        /// </summary>
        /// <param name="newVariableViewModel">New variable view model.</param>
        private void AddAggregateVariableToModel(AggregateVariableViewModel newVariableViewModel)
        {
            Debug.Assert(newVariableViewModel.Model != null);
            this.Model.AddVariable(newVariableViewModel.Model);
        }

        /// <summary>
        /// Add a new domain to the model model.
        /// </summary>
        /// <param name="newDomainViewModel">New domain view model.</param>
        private void AddDomainToModel(DomainViewModel newDomainViewModel)
        {
            Debug.Assert(newDomainViewModel.Model != null);
            this.Model.AddDomain(newDomainViewModel.Model);
        }

        /// <summary>
        /// Add a new constraint to the model model.
        /// </summary>
        /// <param name="newConstraintViewModel">New constraint view model.</param>
        private void AddConstraintToModel(ConstraintViewModel newConstraintViewModel)
        {
            Debug.Assert(newConstraintViewModel.Model != null);
            this.Model.AddConstraint(newConstraintViewModel.Model);
        }

        private void DeleteConstraintFromModel(ConstraintViewModel constraintToDelete)
        {
            Debug.Assert(constraintToDelete.Model != null);
            this.Model.DeleteConstraint(constraintToDelete.Model);
        }

        private void DeleteVariableFromModel(VariableViewModel variableToDelete)
        {
            Debug.Assert(variableToDelete.Model != null);
            this.Model.DeleteVariable(variableToDelete.Model);
        }

        private void DeleteDomainFromModel(DomainViewModel domainToDelete)
        {
            Debug.Assert(domainToDelete.Model != null);
            this.Model.DeleteDomain(domainToDelete.Model);
        }

        /// <summary>
        /// Display a dialog box with a display of all of the model errors.
        /// </summary>
        /// <param name="theModel">Model with errors to display.</param>
        private void DisplayErrorDialog(ModelModel theModel)
        {
            var errorsViewModel = CreateModelErrorsFrom(theModel);
            this.windowManager.ShowDialog(errorsViewModel);
        }
    }
}
