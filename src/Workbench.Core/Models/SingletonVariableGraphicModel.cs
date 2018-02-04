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
        public SingletonVariableGraphicModel(SingletonVariableModel theModel, Point newLocation)
            : base(theModel, newLocation)
        {
        }

        /// <summary>
        /// Initializes a variable with a variable name, location and domain expression.
        /// </summary>
        public SingletonVariableGraphicModel(SingletonVariableModel theModel)
            : base(theModel, new Point())
        {
        }
    }
}
