using System;

namespace DynaApp.Models
{
    [Serializable]
    public class VariableModel : GraphicModel
    {
        /// <summary>
        /// Initializes a variable model with a variable name.
        /// </summary>
        public VariableModel(string variableName)
            : base(variableName)
        {
        }

        /// <summary>
        /// Initializes a variable model with default values.
        /// </summary>
        public VariableModel()
            : base("New variable")
        {
        }

        /// <summary>
        /// Gets or sets the variable domain expression.
        /// </summary>
        public DomainExpressionModel DomainExpression { get; set; }
    }
}
