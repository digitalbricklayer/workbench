using System;

namespace DynaApp.Models
{
    [Serializable]
    public class VariableModel : GraphicModel
    {
        public VariableModel(string variableName)
            : base(variableName)
        {
        }

        public VariableModel()
            : base("New variable")
        {
        }
    }
}
