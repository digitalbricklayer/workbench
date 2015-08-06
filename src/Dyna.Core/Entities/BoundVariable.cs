using System;

namespace Dyna.Core.Entities
{
    /// <summary>
    /// A variable bound with a value.
    /// </summary>
    public class BoundVariable
    {
        /// <summary>
        /// Initialize the bound variable with a variable.
        /// </summary>
        /// <param name="theVariable"></param>
        public BoundVariable(Variable theVariable)
        {
            if (theVariable == null)
                throw new ArgumentNullException("theVariable");
            this.Variable = theVariable;
        }

        /// <summary>
        /// Gets the variable name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.Variable.Name;
            }
        }

        /// <summary>
        /// Gets the model variable.
        /// </summary>
        public Variable Variable { get; private set; }

        /// <summary>
        /// Gets or sets the bound value.
        /// </summary>
        public int Value { get; set; }
    }
}
