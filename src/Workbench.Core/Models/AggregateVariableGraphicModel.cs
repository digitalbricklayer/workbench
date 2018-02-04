using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An aggregate variable graphic.
    /// </summary>
    [Serializable]
    public class AggregateVariableGraphicModel : VariableGraphicModel
    {
        private AggregateVariableModel variable;

        public AggregateVariableGraphicModel(AggregateVariableModel theVariable, Point newVariableLocation)
            : base(theVariable, newVariableLocation)
        {
            Contract.Requires<ArgumentNullException>(theVariable != null);
            this.variable = theVariable;
        }

        /// <summary>
        /// Initializes an aggregate variable with a name, a size and domain expression.
        /// </summary>
        public AggregateVariableGraphicModel(AggregateVariableModel theVariable)
            : base(theVariable, new Point())
        {
            Contract.Requires<ArgumentNullException>(theVariable != null);
            this.variable = theVariable;
        }

        /// <summary>
        /// Gets the aggregate variable model.
        /// </summary>
        public override VariableModel Variable
        {
            get
            {
                return this.variable;
            }
            set
            {
                this.variable = (AggregateVariableModel) value;
                base.Variable = value;
            }
        }

        /// <summary>
        /// Gets the variables in the aggregate.
        /// </summary>
        public IReadOnlyCollection<VariableModel> Variables
        {
            get
            {
                return new ReadOnlyCollection<VariableModel>(this.variable.Variables.ToList()); 
            }
        }

        /// <summary>
        /// Gets a count of the variables in the aggregate.
        /// </summary>
        public int AggregateCount
        {
            get
            {
                return this.variable.AggregateCount;
            }
        }

        /// <summary>
        /// Gets or sets the variable domain expression.
        /// </summary>
        public new VariableDomainExpressionModel DomainExpression
        {
            get { return this.variable.DomainExpression; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);

                this.variable.DomainExpression = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Resize the aggregate variable.
        /// </summary>
        /// <param name="newAggregateSize">New aggregate size.</param>
        public void Resize(int newAggregateSize)
        {
            Contract.Requires<ArgumentOutOfRangeException>(newAggregateSize > 0);

            this.variable.Resize(newAggregateSize);
        }

        /// <summary>
        /// Get the variable at the one based index.
        /// </summary>
        /// <param name="variableIndex">Variable index starts at zero.</param>
        /// <returns>Variable at the index.</returns>
        public VariableModel GetVariableByIndex(int variableIndex)
        {
            Contract.Requires<ArgumentOutOfRangeException>(variableIndex < this.Variables.Count() && variableIndex >= 0);
            return this.variable.GetVariableByIndex(variableIndex);
        }

        /// <summary>
        /// Overrides a variable domain expression to a new domain expression.
        /// </summary>
        /// <param name="variableIndex">Variable index starts at zero.</param>
        /// <param name="newDomainExpression">New domain expression.</param>
        public void OverrideDomainTo(int variableIndex, VariableDomainExpressionModel newDomainExpression)
        {
            Contract.Requires<ArgumentOutOfRangeException>(variableIndex < Variables.Count());
            Contract.Requires<ArgumentNullException>(newDomainExpression != null);

            this.variable.OverrideDomainTo(variableIndex, newDomainExpression);
        }

        /// <summary>
        /// Get the size of the variable.
        /// </summary>
        /// <returns>Size of the variable.</returns>
        public override long GetSize()
        {
            return AggregateCount;
        }
    }
}
