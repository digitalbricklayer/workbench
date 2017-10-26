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

        public AggregateVariableGraphicModel(string newVariableName, Point newVariableLocation, int aggregateSize, VariableDomainExpressionModel theDomainExpression)
            : base(newVariableName, newVariableLocation, theDomainExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(newVariableName));
            Contract.Requires<ArgumentOutOfRangeException>(aggregateSize >= 0);
            Contract.Requires<ArgumentNullException>(theDomainExpression != null);
            this.variable = new AggregateVariableModel(newVariableName, aggregateSize, theDomainExpression);
        }

        /// <summary>
        /// Initializes an aggregate variable with a name, a size and domain expression.
        /// </summary>
        public AggregateVariableGraphicModel(string theVariableName, int aggregateSize, VariableDomainExpressionModel theDomainExpression)
            : base(theVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVariableName));
            Contract.Requires<ArgumentOutOfRangeException>(aggregateSize >= 0);
            Contract.Requires<ArgumentNullException>(theDomainExpression != null);
            this.variable = new AggregateVariableModel(theVariableName, aggregateSize, theDomainExpression);
        }

        /// <summary>
        /// Initializes an aggregate variable with a name, a size and domain expression.
        /// </summary>
        public AggregateVariableGraphicModel(string variableName, int aggregateSize, string theRawDomainExpression)
            : base(variableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(variableName));
            Contract.Requires<ArgumentOutOfRangeException>(aggregateSize >= 0);
            Contract.Requires<ArgumentNullException>(theRawDomainExpression != null);
            this.variable = new AggregateVariableModel(variableName, aggregateSize, theRawDomainExpression);
        }

        /// <summary>
        /// Initializes an aggregate variable with a name and domain expression.
        /// </summary>
        public AggregateVariableGraphicModel(string variableName, VariableDomainExpressionModel theDomainExpression)
            : base(variableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(variableName));
            Contract.Requires<ArgumentNullException>(theDomainExpression != null);

            this.variable = new AggregateVariableModel(variableName, 0, theDomainExpression);
        }

        public AggregateVariableGraphicModel(string newVariableName, Point newVariableLocation)
            : this(newVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(newVariableName));

            this.variable = new AggregateVariableModel(newVariableName);
        }

        /// <summary>
        /// Initialize an aggregate variable with a name.
        /// </summary>
        /// <param name="newName">New variable name.</param>
        public AggregateVariableGraphicModel(string newName)
            : base(newName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(newName));

            this.variable = new AggregateVariableModel(newName);
        }

        /// <summary>
        /// Initialize an aggregate variable with default values.
        /// </summary>
        public AggregateVariableGraphicModel()
        {
            this.variable = new AggregateVariableModel();
        }

#if true
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
#endif

        /// <summary>
        /// Gets the variables in the aggregate.
        /// </summary>
        public IReadOnlyCollection<VariableGraphicModel> Variables
        {
            get
            {
                return new ReadOnlyCollection<VariableGraphicModel>(this.variable.Variables.ToList()); 
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
        public VariableGraphicModel GetVariableByIndex(int variableIndex)
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
