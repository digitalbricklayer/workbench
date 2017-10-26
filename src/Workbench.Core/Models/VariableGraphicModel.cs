using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A variable graphic.
    /// </summary>
    [Serializable]
    public abstract class VariableGraphicModel : GraphicModel
    {
        private VariableModel variable;

        /// <summary>
        /// Initializes a variable with a variable name, location and domain expression.
        /// </summary>
        protected VariableGraphicModel(string variableName, Point newLocation, VariableDomainExpressionModel newVariableExpression)
            : base(variableName, newLocation)
        {
            if (newVariableExpression == null)
                throw new ArgumentNullException(nameof(newVariableExpression));
            Contract.EndContractBlock();
            this.variable = new SingletonVariableModel(variableName, newVariableExpression);
        }

        /// <summary>
        /// Initializes a variable with a variable name and domain expression.
        /// </summary>
        protected VariableGraphicModel(string variableName, VariableDomainExpressionModel theDomainExpression)
            : base(variableName)
        {
            if (theDomainExpression == null)
                throw new ArgumentNullException(nameof(theDomainExpression));
            Contract.EndContractBlock();
            this.variable = new SingletonVariableModel(variableName, theDomainExpression);
        }

        /// <summary>
        /// Initializes a variable with a variable name and domain expression.
        /// </summary>
        protected VariableGraphicModel(string variableName, string theRawDomainExpression)
            : base(variableName)
        {
            this.variable = new SingletonVariableModel(variableName, theRawDomainExpression);
        }

        /// <summary>
        /// Initializes a variable with a variable name.
        /// </summary>
        protected VariableGraphicModel(string variableName)
            : base(variableName)
        {
            this.variable = new SingletonVariableModel(variableName);
        }

        /// <summary>
        /// Initializes a variable with default values.
        /// </summary>
        protected VariableGraphicModel()
            : base("New variable")
        {
            this.variable = new SingletonVariableModel();
        }

        /// <summary>
        /// Gets or sets the variable domain expression.
        /// </summary>
        public VariableDomainExpressionModel DomainExpression
        {
            get { return Variable.DomainExpression; }
            set
            {
                Variable.DomainExpression = value; 
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the variable model.
        /// </summary>
        public virtual VariableModel Variable
        {
            get
            {
                return this.variable;
            }
            set
            {
                this.variable = value;
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
            return Variable.Name;
        }

        /// <summary>
        /// Get the size of the variable.
        /// </summary>
        /// <returns>Size of the variable.</returns>
        public virtual long GetSize()
        {
            return Variable.GetSize();
        }
    }
}
