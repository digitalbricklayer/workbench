using System;

namespace DynaApp.Entities
{
    /// <summary>
    /// A variable with a value.
    /// </summary>
    class BoundVariable
    {
        public BoundVariable(Variable theModelVariable)
        {
            if (theModelVariable == null)
                throw new ArgumentNullException("theModelVariable");
            this.ModelVariable = theModelVariable;
        }

        public Variable ModelVariable { get; private set; }

        /// <summary>
        /// Gets or sets the bound value.
        /// </summary>
        public int Value { get; set; }
    }
}
