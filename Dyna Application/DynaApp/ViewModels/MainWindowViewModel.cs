using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for the main window.
    /// </summary>
    public sealed class MainWindowViewModel : AbstractViewModel
    {
        private readonly ObservableCollection<string> availableDisplayModes 
            = new ObservableCollection<string>{"Model"};
        private string selectedDisplayMode;
        private object selectedDisplayViewModel;

        /// <summary>
        /// Initialize a main windows view model with default values.
        /// </summary>
        public MainWindowViewModel()
        {
            this.Solution = new SolutionViewModel();
            this.Model = new ModelViewModel();
            this.SelectedDisplayMode = "Model";
        }

        /// <summary>
        /// Gets the model displayed in the main window.
        /// </summary>
        public ModelViewModel Model { get; private set; }

        /// <summary>
        /// Gets the solution displayed in the main window.
        /// </summary>
        public SolutionViewModel Solution { get; private set; }

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
        /// Create a new constraint.
        /// </summary>
        /// <param name="newVariableName">New variable name.</param>
        /// <param name="newVariableLocation">New variable location.</param>
        /// <returns>New variable view model.</returns>
        public VariableViewModel CreateVariable(string newVariableName, Point newVariableLocation)
        {
            var newVariable = new VariableViewModel(newVariableName);
            newVariable.X = newVariableLocation.X;
            newVariable.Y = newVariableLocation.Y;

            this.Model.AddVariable(newVariable);

            return newVariable;
        }

        /// <summary>
        /// Create a new constraint.
        /// </summary>
        /// <param name="newDomainName">New constraint name.</param>
        /// <param name="newDomainLocation">New constraint location.</param>
        /// <returns>New constraint view model.</returns>
        public DomainViewModel CreateDomain(string newDomainName, Point newDomainLocation)
        {
            var newDomain = new DomainViewModel(newDomainName);
            newDomain.X = newDomainLocation.X;
            newDomain.Y = newDomainLocation.Y;

            this.Model.AddDomain(newDomain);

            return newDomain;
        }

        /// <summary>
        /// Create a new constraint.
        /// </summary>
        /// <param name="newConstraintName">New constraint name.</param>
        /// <param name="point">New constraint location.</param>
        /// <returns>New constraint view model.</returns>
        private ConstraintViewModel CreateConstraint(string newConstraintName, Point point)
        {
            var newConstraint = new ConstraintViewModel(newConstraintName);
            newConstraint.X = point.X;
            newConstraint.Y = point.Y;

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
        /// Solve the model.
        /// </summary>
        public void SolveModel(Window parentWindow)
        {
            var solveResult = this.Model.Solve(parentWindow);

            if (!solveResult) return;

            // Display the solution...
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
