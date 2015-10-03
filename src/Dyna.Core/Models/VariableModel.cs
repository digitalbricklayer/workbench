using System;

namespace Dyna.Core.Models
{
    [Serializable]
    public class VariableModel : GraphicModel
    {
        /// <summary>
        /// Initializes a variable with a variable name.
        /// </summary>
        public VariableModel(string variableName)
            : base(variableName)
        {
            this.DomainExpression = new VariableDomainExpressionModel();
        }

        /// <summary>
        /// Initializes a variable with a variable name and domain expression.
        /// </summary>
        public VariableModel(string variableName, VariableDomainExpressionModel theDomainExpression)
            : base(variableName)
        {
            if (theDomainExpression == null)
                throw new ArgumentNullException("theDomainExpression");
            this.DomainExpression = theDomainExpression;
        }

        /// <summary>
        /// Initializes a variable with a variable name and domain expression.
        /// </summary>
        public VariableModel(string variableName, string theRawDomainExpression)
            : base(variableName)
        {
            this.DomainExpression = new VariableDomainExpressionModel(theRawDomainExpression);
        }

        /// <summary>
        /// Initializes a variable with default values.
        /// </summary>
        public VariableModel()
            : base("New variable")
        {
            this.DomainExpression = new VariableDomainExpressionModel();
        }

        /// <summary>
        /// Gets or sets the variable domain expression.
        /// </summary>
        public VariableDomainExpressionModel DomainExpression { get; set; }

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        public DomainModel Domain { get; internal set; }

        /// <summary>
        /// Attach the variable to a domain.
        /// </summary>
        /// <param name="domain">A domain.</param>
        public void AttachTo(DomainModel domain)
        {
            if (domain == null)
                throw new ArgumentNullException("domain");
            this.Domain = domain;
        }

        /// <summary>
        /// Returns a string that represents the variable.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
