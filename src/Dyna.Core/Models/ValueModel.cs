using System;

namespace Dyna.Core.Models
{
    /// <summary>
    /// A value bound to a variable.
    /// </summary>
    [Serializable]
    public class ValueModel : ModelBase
    {
        /// <summary>
        /// Initialize the value with a variable.
        /// </summary>
        /// <param name="theVariable"></param>
        public ValueModel(VariableModel theVariable)
        {
            this.Variable = theVariable;
        }

        /// <summary>
        /// Gets or sets the variable that the value is bound to.
        /// </summary>
        public VariableModel Variable { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets the variable name.
        /// </summary>
        public string VariableName
        {
            get { return this.Variable.Name; }
        }
    }
}
