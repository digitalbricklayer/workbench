using System;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A singleton variable can hold a single value constrained by a constraint.
    /// </summary>
    [Serializable]
    public class SingletonVariableModel : VariableModel
    {
        /// <summary>
        /// Initializes a variable with a variable name and domain expression.
        /// </summary>
        public SingletonVariableModel(string variableName, VariableDomainExpressionModel theDomainExpression)
            : base(variableName, theDomainExpression)
        {
        }

        /// <summary>
        /// Initializes a variable with a variable name and domain expression.
        /// </summary>
        public SingletonVariableModel(string variableName, string theRawDomainExpression)
            : base(variableName, theRawDomainExpression)
        {
        }

        /// <summary>
        /// Initializes a variable with a variable name.
        /// </summary>
        public SingletonVariableModel(string variableName)
            : base(variableName)
        {
        }

        /// <summary>
        /// Initializes a variable with default values.
        /// </summary>
        public SingletonVariableModel()
        {
        }

        /// <summary>
        /// Get the size of the variable.
        /// </summary>
        /// <returns>Size of the variable.</returns>
        public override long GetSize()
        {
            return 1;
        }
    }
}
