using System;

namespace DynaApp.Models
{
    [Serializable]
    public class ConstraintModel
    {
        public string Name { get; set; }
        public ConstraintExpressionModel Expression { get; set; }
    }
}
