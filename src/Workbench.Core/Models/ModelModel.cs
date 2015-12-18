using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A model for specifying the problem.
    /// <remarks>Just a very simple finite integer domain at the moment.</remarks>
    /// </summary>
    [Serializable]
    public class ModelModel : AbstractModel
    {
        private readonly ObservableCollection<string> errors = new ObservableCollection<string>();
        private ObservableCollection<VariableModel> variables;
        private ObservableCollection<AggregateVariableModel> aggregates;
        private ObservableCollection<DomainModel> domains;
        private ObservableCollection<ConstraintModel> constraints;
        private string name;

        /// <summary>
        /// Initialize a model with a model name.
        /// </summary>
        /// <param name="theName">Model name.</param>
        public ModelModel(string theName)
            : this()
        {
            if (string.IsNullOrWhiteSpace(theName))
                throw new ArgumentException("theName");
            this.Name = theName;
        }

        /// <summary>
        /// Initialize a model model with default values.
        /// </summary>
        public ModelModel()
        {
            this.Name = string.Empty;
            this.Variables = new ObservableCollection<VariableModel>();
            this.Aggregates = new ObservableCollection<AggregateVariableModel>();
            this.Domains = new ObservableCollection<DomainModel>();
            this.Constraints = new ObservableCollection<ConstraintModel>();
        }

        /// <summary>
        /// Gets or sets the model name.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets and sets the variables.
        /// </summary>
        public ObservableCollection<VariableModel> Variables
        {
            get { return variables; }
            set
            {
                variables = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets and sets the aggregate variables.
        /// </summary>
        public ObservableCollection<AggregateVariableModel> Aggregates
        {
            get { return aggregates; }
            set
            {
                aggregates = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the domains.
        /// </summary>
        public ObservableCollection<DomainModel> Domains
        {
            get { return domains; }
            set
            {
                domains = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the constraints.
        /// </summary>
        public ObservableCollection<ConstraintModel> Constraints
        {
            get { return constraints; }
            set
            {
                constraints = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the model validation errors.
        /// </summary>
        public IEnumerable<String> Errors
        {
            get
            {
                return this.errors;
            }
        }

        /// <summary>
        /// Add a new constraint to the model.
        /// </summary>
        /// <param name="newConstraint">New constraint.</param>
        public void AddConstraint(ConstraintModel newConstraint)
        {
            if (newConstraint == null)
                throw new ArgumentNullException("newConstraint");
            newConstraint.AssignIdentity();
            this.Constraints.Add(newConstraint);
        }

        /// <summary>
        /// Delete the constraint from the model.
        /// </summary>
        /// <param name="constraintToDelete">Constraint to delete.</param>
        public void DeleteConstraint(ConstraintModel constraintToDelete)
        {
            if (constraintToDelete == null)
                throw new ArgumentNullException("constraintToDelete");
            this.Constraints.Remove(constraintToDelete);
        }

        /// <summary>
        /// Add a new variable to the model.
        /// </summary>
        /// <param name="newVariable">New variable.</param>
        public void AddVariable(VariableModel newVariable)
        {
            if (newVariable == null)
                throw new ArgumentNullException("newVariable");
            newVariable.AssignIdentity();
            this.Variables.Add(newVariable);
        }

        /// <summary>
        /// Add a new aggregate variable to the model.
        /// </summary>
        /// <param name="newVariable">New aggregate variable.</param>
        public void AddVariable(AggregateVariableModel newVariable)
        {
            if (newVariable == null)
                throw new ArgumentNullException("newVariable");
            newVariable.AssignIdentity();
            this.Aggregates.Add(newVariable);
        }

        /// <summary>
        /// Delete the variable from the model.
        /// </summary>
        /// <param name="variableToDelete">Variable to delete.</param>
        public void DeleteVariable(VariableModel variableToDelete)
        {
            if (variableToDelete == null)
                throw new ArgumentNullException("variableToDelete");
            this.Variables.Remove(variableToDelete);
        }

        public void AddDomain(DomainModel newDomain)
        {
            if (newDomain == null)
                throw new ArgumentNullException("newDomain");
            newDomain.AssignIdentity();
            this.Domains.Add(newDomain);
        }

        public void AddSharedDomain(DomainModel newDomain)
        {
            if (newDomain == null)
                throw new ArgumentNullException("newDomain");
            if (string.IsNullOrWhiteSpace(newDomain.Name))
                throw new ArgumentException("Shared domains must have a name.", "newDomain");
            this.Domains.Add(newDomain);
        }

        public void RemoveSharedDomain(DomainModel oldDomain)
        {
            if (oldDomain == null)
                throw new ArgumentNullException("oldDomain");
            this.Domains.Add(oldDomain);
        }

        /// <summary>
        /// Delete the domain from the model.
        /// </summary>
        /// <param name="domainToDelete">Domain to delete.</param>
        public void DeleteDomain(DomainModel domainToDelete)
        {
            if (domainToDelete == null)
                throw new ArgumentNullException("domainToDelete");
            this.Domains.Remove(domainToDelete);
        }

        /// <summary>
        /// Get the variable matching the variable name.
        /// </summary>
        /// <param name="theVariableName">The variable name.</param>
        /// <returns>Variable model.</returns>
        public VariableModel GetVariableByName(string theVariableName)
        {
            return this.Variables.FirstOrDefault(variable => variable.Name == theVariableName);
        }

        /// <summary>
        /// Validate the model and ensure consistency between the domains and Variables.
        /// <remarks>Populates errors into the <see cref="Errors"/> collection.</remarks>
        /// </summary>
        /// <returns>True if the model is valid, False if it is not valid.</returns>
        public bool Validate()
        {
            this.errors.Clear();
            var expressionsValid = this.ValidateConstraints();
            if (!expressionsValid) return false;
            return this.ValidateSharedDomains();
        }

        /// <summary>
        /// Get the shared domain matching the given name.
        /// </summary>
        /// <param name="theSharedDomainName">Shared domain name.</param>
        /// <returns>Shared domain matching the name.</returns>
        public DomainModel GetSharedDomainByName(string theSharedDomainName)
        {
            if (string.IsNullOrWhiteSpace(theSharedDomainName))
                throw new ArgumentException("theSharedDomainName");
            return this.Domains.FirstOrDefault(x => x.Name == theSharedDomainName);
        }

        /// <summary>
        /// Create a new model with the given name.
        /// </summary>
        /// <param name="theModelName">Model name.</param>
        /// <returns>Fluent interface context.</returns>
        public static ModelContext Create(string theModelName)
        {
            return new ModelContext(new ModelModel(theModelName));
        }

        private bool ValidateConstraints()
        {
            return this.Constraints.All(ValidateConstraint);
        }

        private bool ValidateConstraint(ConstraintModel aConstraint)
        {
            if (!ValidateConstraintExpression(aConstraint.Expression.Left)) return false;
            if (!ValidateConstraintExpression(aConstraint.Expression.Right)) return false;

            return true;
        }

        private bool ValidateConstraintExpression(Expression theExpression)
        {
            if (theExpression.IsSingleton)
            {
                if (this.Variables.FirstOrDefault(x => x.Name == theExpression.Variable.Name) == null)
                {
                    this.errors.Add(string.Format("Missing singleton variable {0}", theExpression.Variable));
                    return false;
                }
            }
            else if (theExpression.IsAggregate)
            {
                if (this.Aggregates.FirstOrDefault(x => x.Name == theExpression.AggregateReference.IdentifierName) == null)
                {
                    this.errors.Add(string.Format("Missing aggregate variable {0}", theExpression.AggregateReference.IdentifierName));
                    return false;
                }
            }

            return true;
        }

        private bool ValidateSharedDomains()
        {
            foreach (var variable in this.Variables)
            {
                if (variable.DomainExpression == null)
                {
                    this.errors.Add("Missing domain");
                    return false;
                }

                // Make sure the domain is a shared domain...
                if (variable.DomainExpression.DomainReference == null)
                    continue;

                var sharedDomain = this.GetSharedDomainByName(variable.DomainExpression.DomainReference.DomainName);
                if (sharedDomain == null)
                {
                    this.errors.Add(string.Format("Missing shared domain {0}", variable.DomainExpression.DomainReference.DomainName));
                    return false;
                }
            }

            return true;
        }
    }
}