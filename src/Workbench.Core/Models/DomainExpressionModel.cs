using System;
using Workbench.Core.Nodes;
using Workbench.Core.Parsers;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An expression specifying a domain.
    /// </summary>
    [Serializable]
    public class DomainExpressionModel : AbstractModel
    {
        private string text;

        [NonSerialized]
        private SharedDomainExpressionNode node;

        /// <summary>
        /// Initialize a domain expression with a raw domain expression text.
        /// </summary>
        public DomainExpressionModel(string rawDomainExpression)
        {
            Text = rawDomainExpression;
        }

        /// <summary>
        /// Initialize a domain expression with default values.
        /// </summary>
        public DomainExpressionModel()
        {
            Text = string.Empty;
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
                ParseUnit(value);
                OnPropertyChanged();
            }
        }

        public SharedDomainExpressionNode Node
        {
            get { return this.node; }
            private set { this.node = value; }
        }

        /// <summary>
        /// Parse the raw domain expression.
        /// </summary>
        /// <param name="rawExpression">Raw domain expression.</param>
        private void ParseUnit(string rawExpression)
        {
            if (!string.IsNullOrWhiteSpace(rawExpression))
            {
                var domainExpressionParser = new SharedDomainExpressionParser();
                var result = domainExpressionParser.Parse(rawExpression);
                if (result.Status == ParseStatus.Success)
                    Node = result.Root;
                else
                {
                    Node = null;
                }
            }
            else
            {
                Node = null;
            }
        }
    }
}