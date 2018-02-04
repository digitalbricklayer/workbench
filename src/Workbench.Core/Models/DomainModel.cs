using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    [Serializable]
    public class DomainModel : BaseModel
    {
        private DomainExpressionModel expression;

        /// <summary>
        /// Initialize a domain with a domain expression.
        /// </summary>
        public DomainModel(ModelName theName, DomainExpressionModel theExpression)
            : base(theName)
        {
            Contract.Requires<ArgumentNullException>(theExpression != null);
            this.expression = theExpression;
        }

        /// <summary>
        /// Initialize a domain with a domain expression.
        /// </summary>
        public DomainModel(ModelName theName)
            : base(theName)
        {
            Contract.Requires<ArgumentNullException>(theName != null);
            this.expression = new DomainExpressionModel();
        }

        /// <summary>
        /// Initialize a domain with a domain expression.
        /// </summary>
        public DomainModel(DomainExpressionModel theExpression)
            : base(new ModelName())
        {
            Contract.Requires<ArgumentNullException>(theExpression != null);
            this.expression = theExpression;
        }

        /// <summary>
        /// Initialize a domain with default values.
        /// </summary>
        public DomainModel()
            : base(new ModelName())
        {
            Expression = new DomainExpressionModel();
        }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public DomainExpressionModel Expression
        {
            get { return this.expression; }
            set
            {
                this.expression = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Parse a raw expression.
        /// </summary>
        /// <param name="rawExpression">Raw domain expression.</param>
        public void ParseExpression(string rawExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(rawExpression));
            Expression = new DomainExpressionModel(rawExpression);
        }
    }
}
