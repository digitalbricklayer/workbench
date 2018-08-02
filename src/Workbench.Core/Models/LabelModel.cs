using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    [Serializable]
    public abstract class LabelModel
    {
        protected LabelModel(VariableModel theVariable)
        {
            Variable = theVariable;
        }

        /// <summary>
        /// Gets the variable associated with the label.
        /// </summary>
        public VariableModel Variable { get; }

        /// <summary>
        /// Gets the variable name.
        /// </summary>
        public string VariableName
        {
            get
            {
                Contract.Assume(Variable != null);
                return Variable.Name.Text;
            }
        }

        /// <summary>
        /// Gets a text representation of the label.
        /// </summary>
        public abstract string Text { get; }
    }
}