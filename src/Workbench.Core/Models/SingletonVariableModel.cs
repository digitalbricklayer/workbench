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
        public SingletonVariableModel(ModelModel theModel, ModelName variableName, VariableDomainExpressionModel theDomainExpression)
            : base(theModel.Workspace, variableName, theDomainExpression)
        {
        }

        /// <summary>
        /// Initialize a variable with a variable name.
        /// </summary>
        public SingletonVariableModel(ModelModel theModel, ModelName variableName)
            : base(theModel, variableName)
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
