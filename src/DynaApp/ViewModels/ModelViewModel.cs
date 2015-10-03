using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Dyna.Core.Models;
using Dyna.Core.Solver;
using DynaApp.Views;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// A view model for a model.
    /// </summary>
    public sealed class ModelViewModel : AbstractViewModel
    {
        /// <summary>
        /// Initialize a model view model with default values.
        /// </summary>
        public ModelViewModel()
        {
            this.Model = new ModelModel();
            this.Graphics = new ObservableCollection<GraphicViewModel>();
            this.Variables = new ObservableCollection<VariableViewModel>();
            this.Domains = new ObservableCollection<DomainViewModel>();
            this.Constraints = new ObservableCollection<ConstraintViewModel>();
        }

        /// <summary>
        /// Gets the collection of domains in the model.
        /// </summary>
        public ObservableCollection<VariableViewModel> Variables { get; private set; }

        /// <summary>
        /// Gets the collection of domains in the model.
        /// </summary>
        public ObservableCollection<DomainViewModel> Domains { get; private set; }

        /// <summary>
        /// Gets the collection of Variables in the model.
        /// </summary>
        public ObservableCollection<ConstraintViewModel> Constraints { get; private set; }

        /// <summary>
        /// Gets the collection of all graphic items in the model.
        /// </summary>
        public ObservableCollection<GraphicViewModel> Graphics { get; private set; }

        /// <summary>
        /// Gets or sets the model model.
        /// </summary>
        public ModelModel Model { get; set; }

        /// <summary>
        /// Add a new variable to the model.
        /// </summary>
        /// <param name="newVariableViewModel">New variable.</param>
        public void AddVariable(VariableViewModel newVariableViewModel)
        {
            if (newVariableViewModel == null)
                throw new ArgumentNullException("newVariableViewModel");
            this.FixupVariable(newVariableViewModel);
            this.AddVariableToModel(newVariableViewModel);
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
            this.Graphics.Remove(variableToDelete);
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
            this.Graphics.Remove(domainToDelete);
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
            this.Graphics.Remove(constraintToDelete);
            this.DeleteConstraintFromModel(constraintToDelete);
        }

        /// <summary>
        /// Solve the model.
        /// </summary>
        /// <param name="parentWindow">Parent window.</param>
        public SolveResult Solve(Window parentWindow)
        {
            var theModel = this.Model;
            var isModelValid = theModel.Validate();
            if (!isModelValid)
            {
                Trace.Assert(theModel.Errors.Any());

                this.DisplayErrorDialog(parentWindow, theModel);
                return SolveResult.InvalidModel;
            }

            Trace.Assert(!theModel.Errors.Any());

            var solver = new ConstraintSolver();

            return solver.Solve(theModel);
        }

        /// <summary>
        /// Get the variable matching the given name.
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
        /// Reset the contents of the model.
        /// </summary>
        public void Reset()
        {
            this.Variables.Clear();
            this.Constraints.Clear();
            this.Domains.Clear();
        }

        /// <summary>
        /// Fixes up a variable view model into the model.
        /// </summary>
        /// <remarks>
        /// Used when mapping the model to a view model.
        /// </remarks>
        /// <param name="variableViewModel">Variable view model.</param>
        internal void FixupVariable(VariableViewModel variableViewModel)
        {
            if (variableViewModel == null)
                throw new ArgumentNullException("variableViewModel");
            this.Graphics.Add(variableViewModel);
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
            this.Graphics.Add(domainViewModel);
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
            this.Graphics.Add(constraintViewModel);
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
        private void AddVariableToModel(VariableViewModel newVariableViewModel)
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
        /// <param name="parentWindow">Dialog parent window.</param>
        /// <param name="theModel">Model with errors to display.</param>
        private void DisplayErrorDialog(Window parentWindow, ModelModel theModel)
        {
            // TODO: Fix this horendous cludge...
            // If parent window is null, then this is likely to being run inside a test fixture.
            if (parentWindow == null) return;
            var errorWindow = new ModelErrorsWindow
            {
                Owner = parentWindow,
                DataContext = CreateModelErrorsFrom(theModel)
            };
            errorWindow.ShowDialog();
        }
    }
}
