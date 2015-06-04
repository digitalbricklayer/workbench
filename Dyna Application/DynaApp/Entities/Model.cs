using System;
using System.Collections.Generic;
using System.Linq;

namespace DynaApp.Entities
{
    /// <summary>
    /// A model for specifying the problem.
    /// <remarks>Just a very simple finite integer domain at the moment.</remarks>
    /// </summary>
    class Model
    {
        private readonly List<Variable> variables = new List<Variable>();
        private readonly List<Domain> domains = new List<Domain>();
        private readonly List<Constraint> constraints = new List<Constraint>();
        private readonly List<string> errors = new List<string>();

        /// <summary>
        /// Initialize a model with a model name.
        /// </summary>
        /// <param name="theName">Model name.</param>
        public Model(string theName)
        {
            if (string.IsNullOrWhiteSpace(theName))
                throw new ArgumentException("theName");
            this.Name = theName;
        }

        /// <summary>
        /// Initialize a default model.
        /// </summary>
        public Model()
        {
            this.Name = string.Empty;
        }

        /// <summary>
        /// Gets or sets the model name.
        /// </summary>
        public string Name { get; set; }

        public IEnumerable<Variable> Variables
        {
            get { return variables; }
        }

        public IEnumerable<Domain> Domains
        {
            get { return domains; }
        }

        public IEnumerable<Constraint> Constraints
        {
            get { return constraints; }
        }

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        public IEnumerable<String> Errors { get { return this.errors; } }

        public void AddConstraint(Constraint newConstraint)
        {
            if (newConstraint == null)
                throw new ArgumentNullException("newConstraint");
            newConstraint.Model = this;
            this.constraints.Add(newConstraint);
        }

        public void RemoveConstraint(Constraint oldConstraint)
        {
            if (oldConstraint == null)
                throw new ArgumentNullException("oldConstraint");
            this.constraints.Add(oldConstraint);
        }

        public void AddVariable(Variable newVariable)
        {
            if (newVariable == null)
                throw new ArgumentNullException("newVariable");
            newVariable.Model = this;
            this.variables.Add(newVariable);
        }

        public void RemoveVariable(Variable oldVariable)
        {
            if (oldVariable == null)
                throw new ArgumentNullException("oldVariable");
            this.variables.Add(oldVariable);
        }

        public void AddSharedDomain(Domain newDomain)
        {
            if (newDomain == null)
                throw new ArgumentNullException("newDomain");
            if (string.IsNullOrWhiteSpace(newDomain.Name))
                throw new ArgumentException("Shared domains must have a name.", "newDomain");
            newDomain.Model = this;
            this.domains.Add(newDomain);
        }

        public void RemoveSharedDomain(Domain oldDomain)
        {
            if (oldDomain == null)
                throw new ArgumentNullException("oldDomain");
            this.domains.Add(oldDomain);
        }

        /// <summary>
        /// Validate the model and ensure consistency between the variables and constraints.
        /// <remarks>Populates errors into the <see cref="Errors"/> collection.</remarks>
        /// </summary>
        /// <returns>True if the model is valid, False if it is not valid.</returns>
        public bool Validate()
        {
            this.errors.Clear();
            var expressionsValid = this.ValidateConstraintExpressions();
            if (!expressionsValid) return false;
            return this.ValidateSharedDomains();
        }

        /// <summary>
        /// Get the shared domain matching the given name.
        /// </summary>
        /// <param name="theSharedDomainName">Shared domain name.</param>
        /// <returns>Shared domain matching the name.</returns>
        public Domain GetSharedDomainByName(string theSharedDomainName)
        {
            if (string.IsNullOrWhiteSpace(theSharedDomainName))
                throw new ArgumentException("theSharedDomainName");
            return this.domains.FirstOrDefault(x => x.Name == theSharedDomainName);
        }

        /// <summary>
        /// Create a new model with the given name.
        /// </summary>
        /// <param name="theModelName">Model name.</param>
        /// <returns>Fluent interface context.</returns>
        public static ModelContext Create(string theModelName)
        {
            return new ModelContext(new Model(theModelName));
        }

        private bool ValidateConstraintExpressions()
        {
            foreach (var aConstraint in this.Constraints)
            {
                if (this.Variables.FirstOrDefault(x => x.Name == aConstraint.Expression.Left.Name) == null)
                {
                    this.errors.Add(string.Format("Missing variable {0}", aConstraint.Expression.Right.Variable));
                    return false;
                }

                if (aConstraint.Expression.Right.IsVarable)
                {
                    if (this.Variables.FirstOrDefault(x => x.Name == aConstraint.Expression.Right.Variable.Name) == null)
                    {
                        this.errors.Add(string.Format("Missing variable {0}", aConstraint.Expression.Right.Variable));
                        return false;
                    }
                }
            }

            return true;
        }

        private bool ValidateSharedDomains()
        {
            foreach (var variable in this.Variables)
            {
                if (variable.Domain == null)
                {
                    this.errors.Add(string.Format("Missing domain"));
                    return false;
                }
                // Make sure the domain is a shared domain...
                if (string.IsNullOrWhiteSpace(variable.Domain.Name))
                    continue;

                var sharedDomain = this.GetSharedDomainByName(variable.Domain.Name);
                if (sharedDomain == null)
                {
                    this.errors.Add(string.Format("Missing shared domain {0}", variable.Domain.Name));
                    return false;
                }
            }

            return true;
        }
    }
}
