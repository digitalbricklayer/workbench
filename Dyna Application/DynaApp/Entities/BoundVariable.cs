using System;

namespace DynaApp.Entities
{
    /// <summary>
    /// A variable bound with a value.
    /// </summary>
    public class BoundVariable
    {
        /// <summary>
        /// Initialize the bound variable with a variable.
        /// </summary>
        /// <param name="theModelVariable"></param>
        public BoundVariable(Variable theModelVariable)
        {
            if (theModelVariable == null)
                throw new ArgumentNullException("theModelVariable");
            this.ModelVariable = theModelVariable;
        }

        /// <summary>
        /// Gets the variable name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.ModelVariable.Name;
            }
        }

        /// <summary>
        /// Gets the model variable.
        /// </summary>
        public Variable ModelVariable { get; private set; }

        /// <summary>
        /// Gets or sets the bound value.
        /// </summary>
        public int Value { get; set; }
    }
}
