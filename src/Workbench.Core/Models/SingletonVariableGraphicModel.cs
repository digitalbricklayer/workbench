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
            SingletonVariable = theModel;
        }

        /// <summary>
        /// Initializes a variable with a variable name, location and domain expression.
        /// </summary>
        public SingletonVariableGraphicModel(SingletonVariableModel theModel)
            : base(theModel, new Point())
        {
            SingletonVariable = theModel;
        }

        public SingletonVariableModel SingletonVariable { get; private set; }
    }
}
