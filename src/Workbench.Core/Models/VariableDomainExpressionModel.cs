using System;
using Workbench.Core.Nodes;
using Workbench.Core.Parsers;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A variable domain expression model can be either be a reference to a shared 
    /// domain or an inline domain expression.
    /// </summary>
    [Serializable]
    public class VariableDomainExpressionModel : AbstractModel
    {
        private string text;

        [NonSerialized]
        private VariableDomainExpressionNode node;

        /// <summary>
        /// Initialize a variable domain expression with raw expression text.
        /// </summary>
        public VariableDomainExpressionModel(string rawExpression)
        {
            Text = rawExpression;
        }

        /// <summary>
        /// Initialize a variable domain expression with a domain expression unit.
        /// </summary>
        /// <param name="theDomainExpressionUnit">Domain expression unit.</param>
        public VariableDomainExpressionModel(VariableDomainExpressionNode theDomainExpressionUnit)
        {
            Node = theDomainExpressionUnit;
        }

        /// <summary>
        /// Initialize a variable domain expression with default values.
        /// </summary>
        public VariableDomainExpressionModel()
        {
            Text = string.Empty;
        }

        public VariableDomainExpressionNode Node
        {
            get { return this.node; }
            private set { this.node = value; }
        }

        /// <summary>
        /// Gets or sets the variable domain expression text.
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                ParseUnit(this.text);
                OnPropertyChanged();
            }
        }

        public SharedDomainReferenceNode DomainReference
        {
            get
            {
                if (Node != null)
                    return Node.DomainReference;
                return null;
            }
        }

        public DomainExpressionNode InlineDomain
        { 
            get
            {
                if (Node != null)
                    return Node.InlineDomain;
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
                return InlineDomain == null && DomainReference == null;
            }
        }

        /// <summary>
        /// Parse the raw variable domain expression.
        /// </summary>
        /// <param name="rawExpression">Raw variable domain expression.</param>
        private void ParseUnit(string rawExpression)
        {
            if (!string.IsNullOrWhiteSpace(rawExpression))
            {
                var variableDomainExpressionParser = new VariableDomainExpressionParser();
                var result = variableDomainExpressionParser.Parse(rawExpression);
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