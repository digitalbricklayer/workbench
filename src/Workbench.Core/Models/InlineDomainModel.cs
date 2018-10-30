using System;

namespace Workbench.Core.Models
{
    [Serializable]
    public class InlineDomainModel : AbstractModel
    {
        private VariableDomainExpressionModel _expression;

        /// <summary>
        /// Initialize a new inline domain with a domain expression.
        /// </summary>
        public InlineDomainModel(string theExpression)
        {
            Expression = new VariableDomainExpressionModel(theExpression);
        }

        /// <summary>
        /// Initialize a new inline domain with an empty expression.
        /// </summary>
        public InlineDomainModel()
        {
            Expression = new VariableDomainExpressionModel();
        }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public VariableDomainExpressionModel Expression
        {
            get => _expression;
            set
            {
                _expression = value;
                OnPropertyChanged();
            }
        }
    }
}
