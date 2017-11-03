using System;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A singleton variable can hold a single value constrained by zero or more constraints.
    /// </summary>
    [Serializable]
    public class SingletonVariableModel : VariableModel
    {
        /// <summary>
        /// Initialize a variable with a variable name and domain expression.
        /// </summary>
        public SingletonVariableModel(ModelModel theModel, string variableName, VariableDomainExpressionModel theDomainExpression)
            : base(theModel, variableName, theDomainExpression)
        {
        }

        /// <summary>
        /// Initialize a variable with a variable name and domain expression.
        /// </summary>
        public SingletonVariableModel(ModelModel theModel, string variableName, string theRawDomainExpression)
            : base(theModel, variableName, theRawDomainExpression)
        {
        }

        /// <summary>
        /// Initialize a variable with a variable name.
        /// </summary>
        public SingletonVariableModel(ModelModel theModel, string variableName)
            : base(theModel, variableName)
        {
        }

        /// <summary>
        /// Initialize a variable with default values.
        /// </summary>
        public SingletonVariableModel(ModelModel theModel)
            : base(theModel, "A singleton")
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
