using System;

namespace Dyna.Core.Models
{
    /// <summary>
    /// A variable domain expression model can be either be a reference to a shared 
    /// domain or an inline domain expression.
    /// </summary>
    [Serializable]
    public class VariableDomainExpressionModel
    {
        private string text;

        /// <summary>
        /// Initialize a variable domain expression with raw expression text.
        /// </summary>
        public VariableDomainExpressionModel(string rawExpression)
        {
            this.Text = rawExpression;
        }

        public VariableDomainExpressionModel(VariableDomainExpressionUnit theVariableDomainUnit)
        {
            if (theVariableDomainUnit == null)
                throw new ArgumentNullException("theVariableDomainUnit");
            this.Unit = theVariableDomainUnit;
        }

        /// <summary>
        /// Initialize a variable domain expression with default values.
        /// </summary>
        public VariableDomainExpressionModel()
        {
            this.Text = string.Empty;
        }

        /// <summary>
        /// Gets the variable domain expression unit.
        /// </summary>
        public VariableDomainExpressionUnit Unit { get; private set; }

        /// <summary>
        /// Gets or sets the variable domain expression text.
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                this.ParseUnit(value);
            }
        }

        public SharedDomainReference DomainReference
        {
            get
            {
                if (this.Unit != null)
                    return this.Unit.DomainReference;
                return null;
            }
        }

        public DomainExpressionModel InlineDomain
        { 
            get
            {
                if (this.Unit != null)
                    return this.Unit.InlineDomain;
                return null;
            }
        }

        /// <summary>
        /// Gets whether the domain expression has a value.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.InlineDomain == null && this.DomainReference == null;
            }
        }

        /// <summary>
        /// Parse the raw variable domain expression.
        /// </summary>
        /// <param name="rawExpression">Raw variable domain expression.</param>
        private void ParseUnit(string rawExpression)
        {
            if (!string.IsNullOrWhiteSpace(rawExpression))
                this.Unit = VariableDomainGrammar.Parse(rawExpression);
            else
                this.Unit = null;
        }
    }
}