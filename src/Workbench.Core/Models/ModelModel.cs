using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A model for specifying the problem.
    /// <remarks>Just a very simple finite integer domain at the moment.</remarks>
    /// </summary>
    [Serializable]
    public class ModelModel : AbstractModel
    {
        private ObservableCollection<VariableModel> variables;
        private ObservableCollection<VariableModel> singletons;
        private ObservableCollection<AggregateVariableModel> aggregates;
        private ObservableCollection<DomainGraphicModel> domains;
        private ObservableCollection<ConstraintGraphicModel> constraints;
        private string name;

        /// <summary>
        /// Initialize a model with a model name.
        /// </summary>
        /// <param name="theName">Model name.</param>
        public ModelModel(string theName)
            : this()
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theName));
            Name = theName;
        }

        /// <summary>
        /// Initialize a model model with default values.
        /// </summary>
        public ModelModel()
        {
            Name = string.Empty;
            Variables = new ObservableCollection<VariableModel>();
            Singletons = new ObservableCollection<VariableModel>();
            Aggregates = new ObservableCollection<AggregateVariableModel>();
            Domains = new ObservableCollection<DomainGraphicModel>();
            Constraints = new ObservableCollection<ConstraintGraphicModel>();
        }

        /// <summary>
        /// Gets or sets the model name.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets and sets the variables.
        /// </summary>
        public ObservableCollection<VariableModel> Variables
        {
            get { return this.variables; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.variables = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the singleton variable collection.
        /// </summary>
        public ObservableCollection<VariableModel> Singletons
        {
            get { return this.singletons; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.singletons = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets and sets the aggregate variables.
        /// </summary>
        public ObservableCollection<AggregateVariableModel> Aggregates
        {
            get { return this.aggregates; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.aggregates = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the domains.
        /// </summary>
        public ObservableCollection<DomainGraphicModel> Domains
        {
            get { return this.domains; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.domains = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the constraints.
        /// </summary>
        public ObservableCollection<ConstraintGraphicModel> Constraints
        {
            get { return this.constraints; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.constraints = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Add a new constraint to the model.
        /// </summary>
        /// <param name="newConstraint">New constraint.</param>
        public void AddConstraint(ConstraintGraphicModel newConstraint)
        {
            Contract.Requires<ArgumentNullException>(newConstraint != null);
            newConstraint.AssignIdentity();
            this.Constraints.Add(newConstraint);
        }

        /// <summary>
        /// Delete the constraint from the model.
        /// </summary>
        /// <param name="constraintToDelete">Constraint to delete.</param>
        public void DeleteConstraint(ConstraintGraphicModel constraintToDelete)
        {
            Contract.Requires<ArgumentNullException>(constraintToDelete != null);
            Constraints.Remove(constraintToDelete);
        }

        /// <summary>
        /// Add a new variable to the model.
        /// </summary>
        /// <param name="newVariable">New variable.</param>
        public void AddVariable(VariableModel newVariable)
        {
            Contract.Requires<ArgumentNullException>(newVariable != null);
            newVariable.AssignIdentity();
            Variables.Add(newVariable);
            Singletons.Add(newVariable);
        }

        /// <summary>
        /// Add a new aggregate variable to the model.
        /// </summary>
        /// <param name="newVariable">New aggregate variable.</param>
        public void AddVariable(AggregateVariableModel newVariable)
        {
            Contract.Requires<ArgumentNullException>(newVariable != null);
            newVariable.AssignIdentity();
            Variables.Add(newVariable);
            Aggregates.Add(newVariable);
        }

        /// <summary>
        /// Delete the variable from the model.
        /// </summary>
        /// <param name="variableToDelete">Variable to delete.</param>
        public void DeleteVariable(VariableModel variableToDelete)
        {
            Contract.Requires<ArgumentNullException>(variableToDelete != null);
            Variables.Remove(variableToDelete);
        }

        public void AddDomain(DomainGraphicModel newDomain)
        {
            Contract.Requires<ArgumentNullException>(newDomain != null);
            newDomain.AssignIdentity();
            Domains.Add(newDomain);
        }

        public void AddSharedDomain(DomainGraphicModel newDomain)
        {
            Contract.Requires<ArgumentNullException>(newDomain != null);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(newDomain.Name));
            newDomain.AssignIdentity();
            Domains.Add(newDomain);
        }

        public void RemoveSharedDomain(DomainGraphicModel oldDomain)
        {
            Contract.Requires<ArgumentNullException>(oldDomain != null);
            Domains.Add(oldDomain);
        }

        /// <summary>
        /// Delete the domain from the model.
        /// </summary>
        /// <param name="domainToDelete">Domain to delete.</param>
        public void DeleteDomain(DomainGraphicModel domainToDelete)
        {
            Contract.Requires<ArgumentNullException>(domainToDelete != null);
            Domains.Remove(domainToDelete);
        }

        /// <summary>
        /// Get the variable matching the variable name.
        /// </summary>
        /// <param name="theVariableName">The variable name.</param>
        /// <returns>Variable model.</returns>
        public VariableModel GetVariableByName(string theVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVariableName));
            return Variables.FirstOrDefault(variable => variable.Name == theVariableName);
        }

        /// <summary>
        /// Validate the model and ensure consistency between the domains and variables.
        /// </summary>
        /// <returns>True if the model is valid, False if it is not valid.</returns>
        public bool Validate()
        {
            return Validate(new ModelValidationContext());
        }

        /// <summary>
        /// Validate the model and ensure consistency between the domains and variables.
        /// <remarks>Populates errors into the <see cref="ModelValidationContext"/> class.</remarks>
        /// </summary>
        /// <returns>True if the model is valid, False if it is not valid.</returns>
        public bool Validate(ModelValidationContext validateContext)
        {
            var expressionsValid = ValidateConstraints(validateContext);
            if (!expressionsValid) return false;
            return ValidateSharedDomains(validateContext);
        }

        /// <summary>
        /// Get the shared domain matching the given name.
        /// </summary>
        /// <param name="theSharedDomainName">Shared domain name.</param>
        /// <returns>Shared domain matching the name.</returns>
        public DomainGraphicModel GetSharedDomainByName(string theSharedDomainName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theSharedDomainName));
            return Domains.FirstOrDefault(x => x.Name == theSharedDomainName);
        }

        private bool ValidateConstraints(ModelValidationContext validateContext)
        {
            return Constraints.All(aConstraint => ValidateConstraint(aConstraint, validateContext));
        }

        private bool ValidateConstraint(ConstraintGraphicModel aConstraint, ModelValidationContext theContext)
        {
            return aConstraint.Validate(this, theContext);
        }

        private bool ValidateSharedDomains(ModelValidationContext validateContext)
        {
            foreach (var variable in Variables)
            {
                if (variable.DomainExpression == null)
                {
                    validateContext.AddError("Missing domain");
                    return false;
                }

                // Make sure the domain is a shared domain...
                if (variable.DomainExpression.DomainReference == null)
                    continue;

                var sharedDomain = this.GetSharedDomainByName(variable.DomainExpression.DomainReference.DomainName.Name);
                if (sharedDomain == null)
                {
                    validateContext.AddError($"Missing shared domain {variable.DomainExpression.DomainReference.DomainName.Name}");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Solve the model.
        /// </summary>
        /// <returns>Solve result.</returns>
        public SolveResult Solve()
        {
            Contract.Ensures(Contract.Result<SolveResult>() != null);
            using (var solver = new OrToolsSolver())
            {
                return solver.Solve(this);
            }
        }
    }
}
