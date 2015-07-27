using System;

namespace DynaApp.Models
{
    [Serializable]
    public class VariableModel : ConnectableModel
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
