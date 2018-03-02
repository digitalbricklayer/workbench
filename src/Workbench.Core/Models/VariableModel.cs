using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A variable can hold a value constrained by a constraint.
    /// </summary>
    [Serializable]
    public abstract class VariableModel : Model
    {
        private ModelModel model;
        private VariableDomainExpressionModel domainExpression;
        private ModelName name;

        /// <summary>
        /// Initializes a variable with a variable name and domain expression.
        /// </summary>
        public VariableModel(ModelModel theModel, ModelName variableName, VariableDomainExpressionModel theDomainExpression)
            : base(variableName)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(variableName != null);
            Contract.Requires<ArgumentNullException>(theDomainExpression != null);

            Model = theModel;
            this.name = variableName;
            DomainExpression = theDomainExpression;
        }

        /// <summary>
        /// Initializes a variable with a variable name.
        /// </summary>
        public VariableModel(ModelModel theModel, ModelName variableName)
            : base(variableName)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(variableName != null);

            Model = theModel;
            this.name = variableName;
            DomainExpression = new VariableDomainExpressionModel();
        }

        /// <summary>
        /// Gets or sets the variable domain expression.
        /// </summary>
        public VariableDomainExpressionModel DomainExpression
        {
            get { return domainExpression; }
            set
            {
                domainExpression = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Get the model that the variable is assigned.
        /// </summary>
        public ModelModel Model
        {
            get { return this.model; }
            internal set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.model = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Returns a string that represents the variable.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Name.Text;
        }

        /// <summary>
        /// Get the size of the variable.
        /// </summary>
        /// <returns>Size of the variable.</returns>
        public abstract long GetSize();

        /// <summary>
        /// Get the variable domain band.
        /// </summary>
        /// <returns>Tuple with the high, low value.</returns>
        public virtual DomainValue GetVariableBand()
        {
            return VariableBandEvaluator.GetVariableBand(this);
        }
    }
}
