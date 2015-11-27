using System;

namespace Dyna.Core.Models
{
    /// <summary>
    /// A variable can hold a value constrained by a constraint.
    /// </summary>
    [Serializable]
    public class VariableModel : GraphicModel
    {
        private VariableDomainExpressionModel domainExpression;

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
        /// Initializes a variable with a variable name.
        /// </summary>
        public VariableModel(string variableName)
            : base(variableName)
        {
            this.DomainExpression = new VariableDomainExpressionModel();
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
        public VariableDomainExpressionModel DomainExpression
        {
            get { return domainExpression; }
            set
            {
                domainExpression = value; 
                OnPropertyChanged();
            }
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
