using System;

namespace DynaApp.Models
{
    [Serializable]
    public class ValueModel : ModelBase
    {
        public ValueModel(VariableModel variableModel)
        {
            this.Variable = variableModel;
        }

        public ValueModel()
        {
            this.Variable = new VariableModel();
        }

        public VariableModel Variable { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets the variable name.
        /// </summary>
        public string VariableName
        {
            get { return this.Variable.Name; }
        }
    }
}
