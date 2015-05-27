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

        public Model(string theName)
        {
            if (string.IsNullOrWhiteSpace(theName))
                throw new ArgumentException("theName");
            this.Name = theName;
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

        public void AddDomain(Domain newDomain)
        {
            if (newDomain == null)
                throw new ArgumentNullException("newDomain");
            newDomain.Model = this;
            this.domains.Add(newDomain);
        }

        public void RemoveDomain(Domain oldDomain)
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
            return this.ValidateConstraintExpressions();
        }

        public static Model Create(string theModelName)
        {
            return new Model(theModelName);
        }

        public Model WithVariable(string theVariableName)
        {
            this.AddVariable(new Variable(theVariableName));
            return this;
        }

        public Model WithDomain(string theDomainExpression)
        {
            this.AddDomain(new Domain(theDomainExpression));
            return this;
        }

        public Model WithConstraint(string theConstraintExpression)
        {
            this.AddConstraint(new Constraint(theConstraintExpression));
            return this;
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
    }
}
