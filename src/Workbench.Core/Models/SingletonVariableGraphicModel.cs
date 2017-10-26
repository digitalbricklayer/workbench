using System;
using System.Windows;

namespace Workbench.Core.Models
{
    [Serializable]
    public class SingletonVariableGraphicModel : VariableGraphicModel
    {
        /// <summary>
        /// Initializes a variable with a variable name, location and domain expression.
        /// </summary>
        public SingletonVariableGraphicModel(string variableName, Point newLocation, VariableDomainExpressionModel newVariableExpression)
            : base(variableName, newLocation, newVariableExpression)
        {
        }

        /// <summary>
        /// Initializes a variable with a variable name and domain expression.
        /// </summary>
        public SingletonVariableGraphicModel(string variableName, VariableDomainExpressionModel theDomainExpression)
            : base(variableName, theDomainExpression)
        {
        }

        /// <summary>
        /// Initializes a variable with a variable name and domain expression.
        /// </summary>
        public SingletonVariableGraphicModel(string variableName, string theRawDomainExpression)
            : base(variableName, theRawDomainExpression)
        {
        }

        /// <summary>
        /// Initializes a variable with a variable name.
        /// </summary>
        public SingletonVariableGraphicModel(string variableName)
            : base(variableName)
        {
        }

        /// <summary>
        /// Initializes a variable with default values.
        /// </summary>
        public SingletonVariableGraphicModel()
        {
        }
    }
}
