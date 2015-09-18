using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Dyna.Core.Models;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for the workspace where a model can be edited and 
    /// the solution displayed.
    /// </summary>
    public sealed class WorkspaceViewModel : AbstractViewModel
    {
        private readonly ObservableCollection<string> availableDisplayModes;
        private string selectedDisplayMode;
        private object selectedDisplayViewModel;
        private bool isDirty;
        private SolutionViewModel solution;
        private ModelViewModel model;

        /// <summary>
        /// Initialize a workspace view model with default values.
        /// </summary>
        public WorkspaceViewModel()
        {
            this.availableDisplayModes = new ObservableCollection<string> {"Model"};
            this.solution = new SolutionViewModel();
            this.model = new ModelViewModel();
            this.WorkspaceModel = new WorkspaceModel();
            this.SelectedDisplayMode = "Model";
        }

        /// <summary>
        /// Gets or sets the workspace model.
        /// </summary>
        public WorkspaceModel WorkspaceModel { get; set; }

        /// <summary>
        /// Gets or sets the model displayed in the workspace.
        /// </summary>
        public ModelViewModel Model
        {
            get { return model; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                this.model = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the solution displayed in the workspace.
        /// </summary>
        public SolutionViewModel Solution
        {
            get { return this.solution; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                this.solution = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the currently selected display mode.
        /// </summary>
        public string SelectedDisplayMode
        {
            get
            {
                return selectedDisplayMode;
            }
            set
            {
                if (this.selectedDisplayMode == value) return;
                this.selectedDisplayMode = value;
                switch (this.selectedDisplayMode)
                {
                    case "Model":
                        this.SelectedDisplayViewModel = this.Model;
                        break;

                    case "Solution":
                        this.SelectedDisplayViewModel = this.Solution;
                        break;

                    default:
                        throw new NotImplementedException("Unknown display mode.");
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the available display modes. Changes depending upon 
        /// whether the model has a solution or not.
        /// </summary>
        public ObservableCollection<string> AvailableDisplayModes
        {
            get
            {
                return this.availableDisplayModes;
            }
        }

        /// <summary>
        /// Gets the selected display view model.
        /// </summary>
        public object SelectedDisplayViewModel
        {
            get
            {
                return this.selectedDisplayViewModel;
            }
            set
            {
                this.selectedDisplayViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the workspace dirty flag.
        /// </summary>
        public bool IsDirty
        {
            get { return this.isDirty; }
            set
            {
                if (this.isDirty == value) return;
                this.isDirty = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Solve the constraint model.
        /// </summary>
        public void SolveModel(Window parentWindow)
        {
            var solveResult = this.Model.Solve(parentWindow);
            if (!solveResult.IsSuccess) return;
            this.DisplaySolution(solveResult.Solution);
        }

        /// <summary>
        /// Create a new variable.
        /// </summary>
        /// <param name="newVariableName">New variable name.</param>
        /// <param name="newVariableLocation">New variable location.</param>
        /// <returns>New variable view model.</returns>
        public VariableViewModel AddVariable(string newVariableName, Point newVariableLocation)
        {
            var newVariable = new VariableViewModel(newVariableName, newVariableLocation);
            this.Model.AddVariable(newVariable);
            this.IsDirty = true;

            return newVariable;
        }

        /// <summary>
        /// Create a new domain.
        /// </summary>
        /// <param name="newDomainName">New domain name.</param>
        /// <param name="newDomainLocation">New domain location.</param>
        /// <returns>New domain view model.</returns>
        public DomainViewModel AddDomain(string newDomainName, Point newDomainLocation)
        {
            var newDomain = new DomainViewModel(newDomainName, newDomainLocation);
            this.Model.AddDomain(newDomain);
            this.IsDirty = true;

            return newDomain;
        }

        /// <summary>
        /// Create a new constraint.
        /// </summary>
        /// <param name="newConstraintName">New constraint name.</param>
        /// <param name="newLocation">New constraint location.</param>
        /// <returns>New constraint view model.</returns>
        public ConstraintViewModel AddConstraint(string newConstraintName, Point newLocation)
        {
            var newConstraint = new ConstraintViewModel(newConstraintName, newLocation);
            this.Model.AddConstraint(newConstraint);
            this.IsDirty = true;

            return newConstraint;
        }

        /// <summary>
        /// Delete all selected graphic items.
        /// </summary>
        public void DeleteSelectedGraphics()
        {
            this.DeleteSelectedVariables();
            this.DeleteSelectedDomains();
            this.DeleteConstraints();
        }

        /// <summary>
        /// Delete the variable from the view-model.
        /// Also deletes any connections to or from the variable.
        /// </summary>
        public void DeleteVariable(VariableViewModel variable)
        {
            this.Model.DeleteVariable(variable);
            this.IsDirty = true;
        }

        /// <summary>
        /// Reset the contents of the workspace.
        /// </summary>
        public void Reset()
        {
            this.Model.Reset();
            this.Solution.Reset();
            this.IsDirty = true;
        }

        /// <summary>
        /// Display the solution.
        /// </summary>
        /// <param name="theSolution">A valid solution.</param>
        private void DisplaySolution(SolutionModel theSolution)
        {
            this.Solution.Reset();
            var newBoundVariables = new List<ValueViewModel>();
            foreach (var value in theSolution.Values)
            {
                var variable = this.Model.GetVariableByName(value.VariableName);
                var boundVariableViewModel = new ValueViewModel(variable)
                {
                    Value = value.Value
                };
                newBoundVariables.Add(boundVariableViewModel);
            }
            this.Solution.BindTo(newBoundVariables);

            if (!this.AvailableDisplayModes.Contains("Solution"))
                this.AvailableDisplayModes.Add("Solution");
            this.SelectedDisplayMode = "Solution";
        }

        /// <summary>
        /// Delete the currently selected domains from the view-model.
        /// </summary>
        private void DeleteSelectedVariables()
        {
            // Take a copy of the domains list so we can delete domains while iterating.
            var variablesCopy = this.Model.Variables.ToArray();

            foreach (var variable in variablesCopy)
            {
                if (variable.IsSelected)
                {
                    this.DeleteVariable(variable);
                }
            }
        }

        private void DeleteSelectedDomains()
        {
            // Take a copy of the domains list so we can delete domains while iterating.
            var domainCopy = this.Model.Domains.ToArray();

            foreach (var domain in domainCopy)
            {
                if (domain.IsSelected)
                {
                    this.DeleteDomain(domain);
                }
            }
        }

        private void DeleteConstraints()
        {
            // Take a copy of the domains list so we can delete domains while iterating.
            var constraintsCopy = this.Model.Constraints.ToArray();

            foreach (var constraint in constraintsCopy)
            {
                if (constraint.IsSelected)
                {
                    this.DeleteConstraint(constraint);
                }
            }
        }

        private void DeleteDomain(DomainViewModel domain)
        {
            //
            // Remove the variable from the network.
            //
            this.Model.DeleteDomain(domain);
        }

        private void DeleteConstraint(ConstraintViewModel constraint)
        {
            //
            // Remove the variable from the network.
            //
            this.Model.DeleteConstraint(constraint);
        }

        private void SyncSolutionFrom(SolutionViewModel solutionViewModel)
        {
            var newSolutionModel = new SolutionModel();
            foreach (var valueViewModel in solutionViewModel.Values)
            {
                var variableModel = this.WorkspaceModel.Model.GetVariableByName(valueViewModel.VariableName);
                var newValueModel = new ValueModel(variableModel)
                {
                    Value = valueViewModel.Value
                };
                newSolutionModel.AddValue(newValueModel);
            }
            this.WorkspaceModel.Solution = newSolutionModel;
        }
    }
}
