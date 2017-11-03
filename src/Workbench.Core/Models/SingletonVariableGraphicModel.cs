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
        public SingletonVariableGraphicModel(ModelModel theModel, string variableName, Point newLocation, VariableDomainExpressionModel newVariableExpression)
            : base(theModel, variableName, newLocation, newVariableExpression)
        {
        }

        /// <summary>
        /// Initializes a variable with a variable name and domain expression.
        /// </summary>
        public SingletonVariableGraphicModel(ModelModel theModel, string variableName, VariableDomainExpressionModel theDomainExpression)
            : base(theModel, variableName, theDomainExpression)
        {
        }

        /// <summary>
        /// Initializes a variable with a variable name and domain expression.
        /// </summary>
        public SingletonVariableGraphicModel(ModelModel theModel, string variableName, string theRawDomainExpression)
            : base(theModel, variableName, theRawDomainExpression)
        {
        }

        /// <summary>
        /// Initializes a variable with a variable name.
        /// </summary>
        public SingletonVariableGraphicModel(ModelModel theModel, string variableName)
            : base(theModel, variableName)
        {
        }

        /// <summary>
        /// Initializes a variable with default values.
        /// </summary>
        public SingletonVariableGraphicModel(ModelModel theModel)
            : base(theModel)
        {
        }
    }
}
