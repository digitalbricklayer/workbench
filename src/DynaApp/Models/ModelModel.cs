using System;
using System.Collections.Generic;
using System.Linq;

namespace DynaApp.Models
{
    /// <summary>
    /// The model model.
    /// </summary>
    [Serializable]
    public class ModelModel : ModelBase
    {
        public List<VariableModel> Variables { get; set; }
        public List<DomainModel> Domains { get; set; }
        public List<ConstraintModel> Constraints { get; set; }

        /// <summary>
        /// Initialize a model model with default values.
        /// </summary>
        public ModelModel()
        {
            this.Variables = new List<VariableModel>();
            this.Domains = new List<DomainModel>();
            this.Constraints = new List<ConstraintModel>();
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
    }
}