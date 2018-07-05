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
    public class ModelModel : Model
    {
        private ObservableCollection<VariableModel> variables;
        private ObservableCollection<SingletonVariableModel> singletons;
        private ObservableCollection<AggregateVariableModel> aggregates;
        private ObservableCollection<DomainModel> domains;
        private ObservableCollection<ConstraintModel> constraints;

        /// <summary>
        /// Initialize a model with a name.
        /// </summary>
        /// <param name="theName">Model name.</param>
        public ModelModel(ModelName theName)
            : this()
        {
            Name = theName;
        }

        /// <summary>
        /// Initialize a model model with default values.
        /// </summary>
        public ModelModel()
        {
            Name = new ModelName();
            Variables = new ObservableCollection<VariableModel>();
            Singletons = new ObservableCollection<SingletonVariableModel>();
            Aggregates = new ObservableCollection<AggregateVariableModel>();
            Domains = new ObservableCollection<DomainModel>();
            Constraints = new ObservableCollection<ConstraintModel>();
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
        public ObservableCollection<SingletonVariableModel> Singletons
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
        public ObservableCollection<DomainModel> Domains
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
        public ObservableCollection<ConstraintModel> Constraints
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
        public void AddConstraint(ConstraintModel newConstraint)
        {
            Contract.Requires<ArgumentNullException>(newConstraint != null);

            Constraints.Add(newConstraint);
        }

        /// <summary>
        /// Delete the constraint from the model.
        /// </summary>
        /// <param name="constraintToDelete">Constraint to delete.</param>
        public void DeleteConstraint(ConstraintModel constraintToDelete)
        {
            Contract.Requires<ArgumentNullException>(constraintToDelete != null);

            Constraints.Remove(constraintToDelete);
        }

        /// <summary>
        /// Add a new variable to the model.
        /// </summary>
        /// <param name="newVariable">New variable.</param>
        public void AddVariable(SingletonVariableModel newVariable)
        {
            Contract.Requires<ArgumentNullException>(newVariable != null);

            if (!newVariable.HasIdentity)
            {
                newVariable.AssignIdentity();
            }

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

            if (!newVariable.HasIdentity)
            {
                newVariable.AssignIdentity();
            }

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

        public void AddSharedDomain(DomainModel newDomain)
        {
            Contract.Requires<ArgumentNullException>(newDomain != null);
            Contract.Assume(newDomain.Name != null);

            if (!newDomain.HasIdentity)
            {
                newDomain.AssignIdentity();
            }
            Domains.Add(newDomain);
        }

        /// <summary>
        /// Delete the domain from the model.
        /// </summary>
        /// <param name="domainToDelete">Domain to delete.</param>
        public void DeleteDomain(DomainModel domainToDelete)
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

            return Variables.FirstOrDefault(variable => variable.Name.IsEqualTo(theVariableName));
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
            Contract.Requires<ArgumentNullException>(validateContext != null);

            var expressionsValid = ValidateConstraints(validateContext);
            if (!expressionsValid) return false;
            return ValidateSharedDomains(validateContext);
        }

        /// <summary>
        /// Get the shared domain matching the given name.
        /// </summary>
        /// <param name="theSharedDomainName">Shared domain name.</param>
        /// <returns>Shared domain matching the name.</returns>
        public DomainModel GetSharedDomainByName(string theSharedDomainName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theSharedDomainName));
            return Domains.FirstOrDefault(x => x.Name.IsEqualTo(theSharedDomainName));
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

        private bool ValidateConstraints(ModelValidationContext validateContext)
        {
            return Constraints.All(aConstraint => ValidateConstraint(aConstraint, validateContext));
        }

        private bool ValidateConstraint(ConstraintModel aConstraint, ModelValidationContext theContext)
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
    }
}
