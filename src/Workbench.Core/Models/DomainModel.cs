using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    [Serializable]
    public class DomainModel : Model
    {
        private DomainExpressionModel _expression;

        /// <summary>
        /// Initialize a domain with a name and domain expression.
        /// </summary>
        public DomainModel(ModelName theName, DomainExpressionModel theExpression)
            : base(theName)
        {
            Contract.Requires<ArgumentNullException>(theName != null);
            Contract.Requires<ArgumentNullException>(theExpression != null);
            _expression = theExpression;
        }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public DomainExpressionModel Expression
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
