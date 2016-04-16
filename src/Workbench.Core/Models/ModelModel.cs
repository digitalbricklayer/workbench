using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Nodes;
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
        private readonly ObservableCollection<string> errors = new ObservableCollection<string>();
        private ObservableCollection<VariableModel> variables;
        private ObservableCollection<VariableModel> singletons;
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
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theName));
            this.Name = theName;
        }

        /// <summary>
        /// Initialize a model model with default values.
        /// </summary>
        public ModelModel()
        {
            this.Name = string.Empty;
            this.Variables = new ObservableCollection<VariableModel>();
            this.Singletons = new ObservableCollection<VariableModel>();
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
                Contract.Requires<ArgumentNullException>(value != null);
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
                Contract.Requires<ArgumentNullException>(value != null);
                variables = value;
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
            get { return aggregates; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
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
                Contract.Requires<ArgumentNullException>(value != null);
                domains = value;
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
        /// Gets the model validation errors.
        /// </summary>
        public IReadOnlyCollection<string> Errors
        {
            get
            {
                Contract.Assume(this.errors != null);
                return this.errors;
            }
        }

        /// <summary>
        /// Add a new constraint to the model.
        /// </summary>
        /// <param name="newConstraint">New constraint.</param>
        public void AddConstraint(ConstraintModel newConstraint)
        {
            Contract.Requires<ArgumentNullException>(newConstraint != null);
            newConstraint.AssignIdentity();
            this.Constraints.Add(newConstraint);
        }

        /// <summary>
        /// Delete the constraint from the model.
        /// </summary>
        /// <param name="constraintToDelete">Constraint to delete.</param>
        public void DeleteConstraint(ExpressionConstraintModel constraintToDelete)
        {
            Contract.Requires<ArgumentNullException>(constraintToDelete != null);
            this.Constraints.Remove(constraintToDelete);
        }

        /// <summary>
        /// Add a new variable to the model.
        /// </summary>
        /// <param name="newVariable">New variable.</param>
        public void AddVariable(VariableModel newVariable)
        {
            Contract.Requires<ArgumentNullException>(newVariable != null);
            newVariable.AssignIdentity();
            this.Variables.Add(newVariable);
            this.Singletons.Add(newVariable);
        }

        /// <summary>
        /// Add a new aggregate variable to the model.
        /// </summary>
        /// <param name="newVariable">New aggregate variable.</param>
        public void AddVariable(AggregateVariableModel newVariable)
        {
            Contract.Requires<ArgumentNullException>(newVariable != null);
            newVariable.AssignIdentity();
            this.Variables.Add(newVariable);
            this.Aggregates.Add(newVariable);
        }

        /// <summary>
        /// Delete the variable from the model.
        /// </summary>
        /// <param name="variableToDelete">Variable to delete.</param>
        public void DeleteVariable(VariableModel variableToDelete)
        {
            Contract.Requires<ArgumentNullException>(variableToDelete != null);
            this.Variables.Remove(variableToDelete);
        }

        public void AddDomain(DomainModel newDomain)
        {
            Contract.Requires<ArgumentNullException>(newDomain != null);
            newDomain.AssignIdentity();
            this.Domains.Add(newDomain);
        }

        public void AddSharedDomain(DomainModel newDomain)
        {
            Contract.Requires<ArgumentNullException>(newDomain != null);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(newDomain.Name));
            newDomain.AssignIdentity();
            this.Domains.Add(newDomain);
        }

        public void RemoveSharedDomain(DomainModel oldDomain)
        {
            Contract.Requires<ArgumentNullException>(oldDomain != null);
            this.Domains.Add(oldDomain);
        }

        /// <summary>
        /// Delete the domain from the model.
        /// </summary>
        /// <param name="domainToDelete">Domain to delete.</param>
        public void DeleteDomain(DomainModel domainToDelete)
        {
            Contract.Requires<ArgumentNullException>(domainToDelete != null);
            this.Domains.Remove(domainToDelete);
        }

        /// <summary>
        /// Get the variable matching the variable name.
        /// </summary>
        /// <param name="theVariableName">The variable name.</param>
        /// <returns>Variable model.</returns>
        public VariableModel GetVariableByName(string theVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVariableName));
            return this.Variables.FirstOrDefault(variable => variable.Name == theVariableName);
        }

        /// <summary>
        /// Validate the model and ensure consistency between the domains and Variables.
        /// <remarks>Populates errors into the <see cref="Errors"/> collection.</remarks>
        /// </summary>
        /// <returns>True if the model is valid, False if it is not valid.</returns>
        public bool Validate()
        {
            Contract.Assume(this.errors != null);
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
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theSharedDomainName));
            return this.Domains.FirstOrDefault(x => x.Name == theSharedDomainName);
        }

        private bool ValidateConstraints()
        {
            return this.Constraints.All(ValidateConstraint);
        }

        private bool ValidateConstraint(ConstraintModel aConstraint)
        {
            var expressionConstraint = aConstraint as ExpressionConstraintModel;
            if (expressionConstraint == null) return true;
            if (!ValidateConstraintExpression(expressionConstraint.Expression.Node.InnerExpression.LeftExpression)) return false;
            if (!ValidateConstraintExpression(expressionConstraint.Expression.Node.InnerExpression.RightExpression)) return false;

            return true;
        }

        private bool ValidateConstraintExpression(ExpressionNode theExpression)
        {
            if (theExpression.IsSingletonReference)
            {
                var singletonReference = (SingletonVariableReferenceNode) theExpression.InnerExpression;
                if (this.Variables.FirstOrDefault(x => x.Name == singletonReference.VariableName) == null)
                {
                    this.errors.Add(string.Format("Missing singleton variable {0}", singletonReference.VariableName));
                    return false;
                }
            }
            else if (theExpression.IsAggregateReference)
            {
                var aggregateReference = (AggregateVariableReferenceNode)theExpression.InnerExpression;
                if (this.Aggregates.FirstOrDefault(x => x.Name == aggregateReference.VariableName) == null)
                {
                    this.errors.Add(string.Format("Missing aggregate variable {0}", aggregateReference.VariableName));
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

        /// <summary>
        /// Solve the model.
        /// </summary>
        /// <returns>Solve result.</returns>
        public SolveResult Solve()
        {
            Contract.Ensures(Contract.Result<SolveResult>() != null);
            var solver = new OrToolsSolver();
            return solver.Solve(this);
        }
    }
}
