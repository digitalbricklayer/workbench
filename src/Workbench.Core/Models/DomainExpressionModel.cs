using System;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An expression specifying a domain.
    /// </summary>
    [Serializable]
    public class DomainExpressionModel : AbstractModel
    {
        private string text;

        /// <summary>
        /// Initialize a domain expression with a domain expression unit.
        /// </summary>
        /// <param name="theDomainExpressionUnit">Domain expression unit.</param>
        public DomainExpressionModel(DomainExpressionUnit theDomainExpressionUnit)
        {
            this.Unit = theDomainExpressionUnit;
        }

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
        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                this.ParseUnit(value);
                OnPropertyChanged();
            }
        }

        public DomainExpressionUnit Unit { get; private set; }

        public int UpperBand
        {
            get
            {
                return this.Unit.UpperBand;
            }
        }

        public int LowerBand
        {
            get
            {
                return this.Unit.LowerBand;
            }
        }

        /// <summary>
        /// Gets the size of the range.
        /// </summary>
        public int Size
        {
            get
            {
                return this.Unit.Size;
            }
        }

        /// <summary>
        /// Parse the raw domain expression.
        /// </summary>
        /// <param name="rawExpression">Raw domain expression.</param>
        private void ParseUnit(string rawExpression)
        {
            if (!string.IsNullOrWhiteSpace(rawExpression))
                this.Unit = DomainGrammar.Parse(rawExpression);
            else
                this.Unit = null;
        }
    }
}