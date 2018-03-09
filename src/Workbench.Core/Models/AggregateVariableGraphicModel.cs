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
        public AggregateVariableGraphicModel(AggregateVariableModel theVariable, Point newVariableLocation)
            : base(theVariable, newVariableLocation)
        {
            Contract.Requires<ArgumentNullException>(theVariable != null);

            AggregateVariable = theVariable;
        }

        /// <summary>
        /// Initializes an aggregate variable with a name, a size and domain expression.
        /// </summary>
        public AggregateVariableGraphicModel(AggregateVariableModel theVariable)
            : base(theVariable, new Point())
        {
            Contract.Requires<ArgumentNullException>(theVariable != null);

            AggregateVariable = theVariable;
        }

        /// <summary>
        /// Gets the variables in the aggregate.
        /// </summary>
        public IReadOnlyCollection<VariableModel> Variables
        {
            get
            {
                return new ReadOnlyCollection<VariableModel>(AggregateVariable.Variables.ToList()); 
            }
        }

        /// <summary>
        /// Gets a count of the variables in the aggregate.
        /// </summary>
        public int AggregateCount
        {
            get
            {
                return AggregateVariable.AggregateCount;
            }
        }

        /// <summary>
        /// Gets or sets the variable domain expression.
        /// </summary>
        public new VariableDomainExpressionModel DomainExpression
        {
            get { return AggregateVariable.DomainExpression; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);

                AggregateVariable.DomainExpression = value;
                OnPropertyChanged();
            }
        }

        public AggregateVariableModel AggregateVariable { get; private set; }

        /// <summary>
        /// Resize the aggregate variable.
        /// </summary>
        /// <param name="newAggregateSize">New aggregate size.</param>
        public void Resize(int newAggregateSize)
        {
            Contract.Requires<ArgumentOutOfRangeException>(newAggregateSize > 0);

            AggregateVariable.Resize(newAggregateSize);
        }

        /// <summary>
        /// Get the variable at the index.
        /// </summary>
        /// <param name="variableIndex">Variable index starts at zero.</param>
        /// <returns>Variable at the index.</returns>
        public VariableModel GetVariableByIndex(int variableIndex)
        {
            Contract.Requires<ArgumentOutOfRangeException>(variableIndex < Variables.Count && variableIndex >= 0);
            return AggregateVariable.GetVariableByIndex(variableIndex);
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

            AggregateVariable.OverrideDomainTo(variableIndex, newDomainExpression);
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
