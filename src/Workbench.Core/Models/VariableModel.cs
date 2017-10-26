using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A variable can hold a value constrained by a constraint.
    /// </summary>
    [Serializable]
    public abstract class VariableModel : AbstractModel
    {
        private VariableDomainExpressionModel domainExpression;
        private string name;

        /// <summary>
        /// Initializes a variable with a variable name and domain expression.
        /// </summary>
        public VariableModel(string variableName, VariableDomainExpressionModel theDomainExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(variableName));
            Contract.Requires<ArgumentNullException>(theDomainExpression != null);

            this.name = variableName;
            DomainExpression = theDomainExpression;
        }

        /// <summary>
        /// Initializes a variable with a variable name and domain expression.
        /// </summary>
        public VariableModel(string variableName, string theRawDomainExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(variableName));
            Contract.Requires<ArgumentNullException>(theRawDomainExpression != null);

            this.name = variableName;
            DomainExpression = new VariableDomainExpressionModel(theRawDomainExpression);
        }

        /// <summary>
        /// Initializes a variable with a variable name.
        /// </summary>
        public VariableModel(string variableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(variableName));

            this.name = variableName;
            DomainExpression = new VariableDomainExpressionModel();
        }

        /// <summary>
        /// Initializes a variable with default values.
        /// </summary>
        public VariableModel()
        {
            this.name = "New variable";
            DomainExpression = new VariableDomainExpressionModel();
        }

        /// <summary>
        /// Gets the name of the aggregate variable.
        /// </summary>
        public virtual string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(value));
                this.name = value;
                OnPropertyChanged();
            }
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
            return Name;
        }

        /// <summary>
        /// Get the size of the variable.
        /// </summary>
        /// <returns>Size of the variable.</returns>
        public virtual long GetSize()
        {
            return 1;
        }
    }
}
