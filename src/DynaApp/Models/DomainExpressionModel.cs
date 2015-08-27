using System;

namespace DynaApp.Models
{
    /// <summary>
    /// An expression specifying a domain.
    /// </summary>
    /// <remarks>
    /// Can be either a name referencing a seperate shared 
    /// domain, or an expression specifying a domain for the variable.
    /// </remarks>
    [Serializable]
    public class DomainExpressionModel
    {
        /// <summary>
        /// Initialize a domain expression with a raw domain expression text.
        /// </summary>
        public DomainExpressionModel(string rawDomainExpression)
        {
            this.Text = rawDomainExpression;
        }

        /// <summary>
        /// Initialize a domain expression with default values.
        /// </summary>
        public DomainExpressionModel()
        {
            this.Text = string.Empty;
        }

        /// <summary>
        /// Gets or sets the expression text.
        /// </summary>
        public string Text { get; set; }
    }
}