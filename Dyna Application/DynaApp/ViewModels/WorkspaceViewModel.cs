using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DynaApp.Entities;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for the workspace where a model can be edited and 
    /// the solution displayed.
    /// </summary>
    public sealed class WorkspaceViewModel : AbstractViewModel
    {
        private readonly ObservableCollection<string> availableDisplayModes
            = new ObservableCollection<string> { "Model" };
        private string selectedDisplayMode;
        private object selectedDisplayViewModel;
        private bool isDirty;
        private SolutionViewModel solution;
        private ModelViewModel model;

        public WorkspaceViewModel()
        {
            this.solution = new SolutionViewModel();
            this.model = new ModelViewModel();
            this.SelectedDisplayMode = "Model";
        }

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
                OnPropertyChanged("Model");
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
                OnPropertyChanged("Solution");
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
                OnPropertyChanged("SelectedDisplayMode");
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
            private set
            {
                this.selectedDisplayViewModel = value;
                OnPropertyChanged("SelectedDisplayViewModel");
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
                OnPropertyChanged("IsDirty");
            }
        }

        /// <summary>
        /// Solve the model.
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
        public VariableViewModel CreateVariable(string newVariableName, Point newVariableLocation)
        {
            var newVariable = new VariableViewModel(newVariableName, newVariableLocation);

            this.Model.AddVariable(newVariable);

            return newVariable;
        }

        /// <summary>
        /// Create a new domain.
        /// </summary>
        /// <param name="newDomainName">New domain name.</param>
        /// <param name="newDomainLocation">New domain location.</param>
        /// <returns>New domain view model.</returns>
        public DomainViewModel CreateDomain(string newDomainName, Point newDomainLocation)
        {
            var newDomain = new DomainViewModel(newDomainName, newDomainLocation);

            this.Model.AddDomain(newDomain);

            return newDomain;
        }

        /// <summary>
        /// Create a new constraint.
        /// </summary>
        /// <param name="newConstraintName">New constraint name.</param>
        /// <param name="newLocation">New constraint location.</param>
        /// <returns>New constraint view model.</returns>
        public ConstraintViewModel CreateConstraint(string newConstraintName, Point newLocation)
        {
            var newConstraint = new ConstraintViewModel(newConstraintName, newLocation);

            this.Model.AddConstraint(newConstraint);

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
            //
            // Remove all connections attached to the variable.
            //
            foreach (var connectionViewModel in variable.AttachedConnections)
                this.Model.Connections.Remove(connectionViewModel);

            //
            // Remove the variable from the network.
            //
            this.Model.Variables.Remove(variable);
        }

        /// <summary>
        /// Reset the contents of the workspace.
        /// </summary>
        public void Reset()
        {
            this.Model.Reset();
            this.Solution.Reset();
        }

        /// <summary>
        /// Display the solution.
        /// </summary>
        /// <param name="theSolution">A valid solution.</param>
        private void DisplaySolution(Solution theSolution)
        {
            this.Solution.Reset();
            var newBoundVariables = new List<ValueViewModel>();
            foreach (var boundVariable in theSolution.BoundVariables)
            {
                var variable = this.Model.GetVariableByName(boundVariable.Name);
                var boundVariableViewModel = new ValueViewModel(variable)
                {
                    Value = boundVariable.Value
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
                    DeleteVariable(variable);
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
                    DeleteDomain(domain);
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
                    DeleteConstraint(constraint);
                }
            }
        }

        private void DeleteDomain(DomainViewModel domain)
        {
            //
            // Remove the variable from the network.
            //
            this.Model.Domains.Remove(domain);
        }

        private void DeleteConstraint(ConstraintViewModel constraint)
        {
            //
            // Remove the variable from the network.
            //
            this.Model.Constraints.Remove(constraint);
        }
    }
}
