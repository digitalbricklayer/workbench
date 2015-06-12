using System.Linq;
using System.Windows;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for the main window.
    /// </summary>
    public sealed class MainWindowViewModel : AbstractModelBase
    {
        /// <summary>
        /// Initialize a main windows view model with default values.
        /// </summary>
        public MainWindowViewModel()
        {
            this.Model = new ModelViewModel();
            var variable1 = this.CreateVariable("Jack", new Point(10, 10));
            this.CreateVariable("Bob", new Point(200, 10));
            var domainX = this.CreateDomain("X", new Point(10, 80));
            var constraintY = this.CreateConstraint("Y", new Point(10, 170));
            this.Model.Connect(variable1, domainX);
            this.Model.Connect(variable1, constraintY);
        }

        /// <summary>
        /// Gets the model displayed in the main window.
        /// </summary>
        public ModelViewModel Model { get; private set; }

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
