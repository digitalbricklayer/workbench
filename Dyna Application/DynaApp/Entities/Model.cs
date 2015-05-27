using System;
using System.Collections.Generic;

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
    }
}
