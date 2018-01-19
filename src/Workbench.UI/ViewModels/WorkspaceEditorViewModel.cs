using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution designer.
    /// </summary>
    public sealed class WorkspaceEditorViewModel : Conductor<GraphicViewModel>.Collection.AllActive
    {
        private DisplayModel model;

        /// <summary>
        /// Initialize a solution designer view model with default values.
        /// </summary>
        public WorkspaceEditorViewModel(DisplayModel theDisplay, ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theDisplay != null);
            Contract.Requires<ArgumentNullException>(theModel != null);

            Model = theDisplay;
            ModelModel = theModel;
            Variables = new BindableCollection<VariableViewModel>();
            Domains = new BindableCollection<DomainViewModel>();
            Constraints = new BindableCollection<ConstraintViewModel>();
            Visualizers = new BindableCollection<EditorViewModel>();
        }

        /// <summary>
        /// Gets or sets the model model.
        /// </summary>
        public ModelModel ModelModel { get; set; }

        /// <summary>
        /// Gets the visualizer model.
        /// </summary>
        public DisplayModel Model
        {
            get { return this.model; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.model = value;
                NotifyOfPropertyChange();
            }
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
        /// Gets all of the visualizers.
        /// </summary>
        public IObservableCollection<EditorViewModel> Visualizers { get; private set; }

        /// <summary>
        /// Add a variable visualizer.
        /// </summary>
        /// <param name="newVisualizer">New variable visualizer.</param>
        public void AddVisualizer(EditorViewModel newVisualizer)
        {
            Contract.Requires<ArgumentNullException>(newVisualizer != null);
            Model.AddVisualizer(newVisualizer.Model);
            FixupVisualizer(newVisualizer);
        }

        /// <summary>
        /// Add a new singleton variable to the model.
        /// </summary>
        /// <param name="newVariableViewModel">New variable.</param>
        public void AddSingletonVariable(SingletonVariableViewModel newVariableViewModel)
        {
            if (newVariableViewModel == null)
                throw new ArgumentNullException("newVariableViewModel");

            FixupSingletonVariable(newVariableViewModel);
            AddSingletonVariableToModel(newVariableViewModel);
        }

        /// <summary>
        /// Add a new aggregate variable to the model.
        /// </summary>
        /// <param name="newVariableViewModel">New variable.</param>
        public void AddAggregateVariable(AggregateVariableViewModel newVariableViewModel)
        {
            if (newVariableViewModel == null)
                throw new ArgumentNullException("newVariableViewModel");

            FixupAggregateVariable(newVariableViewModel);
            AddAggregateVariableToModel(newVariableViewModel);
        }

        /// <summary>
        /// Add a new domain to the model.
        /// </summary>
        /// <param name="newDomainViewModel">New domain.</param>
        public void AddDomain(DomainViewModel newDomainViewModel)
        {
            if (newDomainViewModel == null)
                throw new ArgumentNullException("newDomainViewModel");
            FixupDomain(newDomainViewModel);
            AddDomainToModel(newDomainViewModel);
        }

        /// <summary>
        /// Add a new constraint to the model.
        /// </summary>
        /// <param name="newConstraintViewModel">New constraint.</param>
        public void AddConstraint(ConstraintViewModel newConstraintViewModel)
        {
            if (newConstraintViewModel == null)
                throw new ArgumentNullException("newConstraintViewModel");
            FixupConstraint(newConstraintViewModel);
            AddConstraintToModel(newConstraintViewModel);
        }

        /// <summary>
        /// Delete the variable.
        /// </summary>
        /// <param name="variableToDelete">Variable to delete.</param>
        public void DeleteVariable(VariableViewModel variableToDelete)
        {
            if (variableToDelete == null)
                throw new ArgumentNullException("variableToDelete");
            Variables.Remove(variableToDelete);
            DeactivateItem(variableToDelete, close: true);
            DeleteVariableFromModel(variableToDelete);
        }

        /// <summary>
        /// Delete the domain.
        /// </summary>
        /// <param name="domainToDelete">Domain to delete.</param>
        public void DeleteDomain(DomainViewModel domainToDelete)
        {
            if (domainToDelete == null)
                throw new ArgumentNullException("domainToDelete");
            Domains.Remove(domainToDelete);
            DeactivateItem(domainToDelete, close: true);
            DeleteDomainFromModel(domainToDelete);
        }

        /// <summary>
        /// Delete the constraint.
        /// </summary>
        /// <param name="constraintToDelete">Constraint to delete.</param>
        public void DeleteConstraint(ConstraintViewModel constraintToDelete)
        {
            if (constraintToDelete == null)
                throw new ArgumentNullException("constraintToDelete");
            Constraints.Remove(constraintToDelete);
            DeactivateItem(constraintToDelete, close: true);
            DeleteConstraintFromModel(constraintToDelete);
        }

        /// <summary>
        /// Fixes up a visualizer view model into the design view model.
        /// </summary>
        /// <remarks>
        /// Used when mapping the model to a view model.
        /// </remarks>
        /// <param name="newVisualizerViewModel">Visualizers design view model.</param>
        internal void FixupVisualizer(EditorViewModel newVisualizerViewModel)
        {
            Contract.Requires<ArgumentNullException>(newVisualizerViewModel != null);
            Visualizers.Add(newVisualizerViewModel);
            ActivateItem(newVisualizerViewModel);
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
        internal void FixupAggregateVariable(AggregateVariableViewModel variableViewModel)
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
        internal void FixupDomain(DomainViewModel domainViewModel)
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
        internal void FixupConstraint(ConstraintViewModel constraintViewModel)
        {
            Contract.Requires<ArgumentNullException>(constraintViewModel != null);
            Constraints.Add(constraintViewModel);
            ActivateItem(constraintViewModel);
        }

        internal void Reset()
        {
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
            return Variables.FirstOrDefault(_ => _.Name == variableName);
        }

        /// <summary>
        /// Get the constraint with the constraint name.
        /// </summary>
        /// <param name="constraintName">Name of the constraint.</param>
        /// <returns>Constraint view model matching the name.</returns>
        public ConstraintViewModel GetConstraintByName(string constraintName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(constraintName));
            return Constraints.FirstOrDefault(_ => _.Name == constraintName);
        }

        /// <summary>
        /// Get all selected aggregate variables.
        /// </summary>
        /// <returns>All selected variables.</returns>
        public IList<VariableViewModel> GetSelectedAggregateVariables()
        {
            return Variables.Where(_ => _.IsSelected && _.IsAggregate)
                            .ToList();
        }

        /// <summary>
        /// Add a new variable to the model model.
        /// </summary>
        /// <param name="newVariableViewModel">New variable view model.</param>
        private void AddSingletonVariableToModel(SingletonVariableViewModel newVariableViewModel)
        {
            Contract.Assert(newVariableViewModel.Model != null);
            ModelModel.AddVariable((SingletonVariableGraphicModel)newVariableViewModel.Model);
        }

        /// <summary>
        /// Add a new variable to the model model.
        /// </summary>
        /// <param name="newVariableViewModel">New variable view model.</param>
        private void AddAggregateVariableToModel(AggregateVariableViewModel newVariableViewModel)
        {
            Contract.Assert(newVariableViewModel.Model != null);
            ModelModel.AddVariable(newVariableViewModel.Model);
        }

        /// <summary>
        /// Add a new domain to the model model.
        /// </summary>
        /// <param name="newDomainViewModel">New domain view model.</param>
        private void AddDomainToModel(DomainViewModel newDomainViewModel)
        {
            Contract.Assert(newDomainViewModel.Model != null);
            ModelModel.AddDomain(newDomainViewModel.Model);
        }

        /// <summary>
        /// Add a new constraint to the model model.
        /// </summary>
        /// <param name="newConstraintViewModel">New constraint view model.</param>
        private void AddConstraintToModel(ConstraintViewModel newConstraintViewModel)
        {
            Contract.Assert(newConstraintViewModel.Model != null);
            ModelModel.AddConstraint(newConstraintViewModel.Model);
        }

        private void DeleteConstraintFromModel(ConstraintViewModel constraintToDelete)
        {
            Contract.Assert(constraintToDelete.Model != null);
            ModelModel.DeleteConstraint(constraintToDelete.Model);
        }

        private void DeleteVariableFromModel(VariableViewModel variableToDelete)
        {
            Contract.Assert(variableToDelete.Model != null);
            ModelModel.DeleteVariable(variableToDelete.Model);
        }

        private void DeleteDomainFromModel(DomainViewModel domainToDelete)
        {
            Contract.Assert(domainToDelete.Model != null);
            ModelModel.DeleteDomain(domainToDelete.Model);
        }

        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(Model != null);
            Contract.Invariant(ModelModel != null);
            Contract.Invariant(Constraints != null);
            Contract.Invariant(Variables != null);
            Contract.Invariant(Domains != null);
            Contract.Invariant(Visualizers != null);
        }
    }
}
