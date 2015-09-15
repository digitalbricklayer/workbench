using System;

namespace DynaApp.Models
{
    /// <summary>
    /// A variable domain expression model can be either a reference to a shared 
    /// domain or an inline domain expression.
    /// </summary>
    [Serializable]
    public class VariableDomainExpressionModel
    {
        /// <summary>
        /// Initialize a variable domain expression with raw expression text.
        /// </summary>
        public VariableDomainExpressionModel(string rawExpression)
        {
            this.Text = rawExpression;
        }

        /// <summary>
        /// Initialize a variable domain expression with default values.
        /// </summary>
        public VariableDomainExpressionModel()
        {
            this.Text = string.Empty;
        }

        /// <summary>
        /// Gets or sets the variable domain expression text.
        /// </summary>
        public string Text { get; set; }
    }
}