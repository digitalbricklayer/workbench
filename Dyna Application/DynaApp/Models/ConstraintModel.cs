using System;

namespace DynaApp.Models
{
    [Serializable]
    public class ConstraintModel : ConnectableModel
    {
        public ConstraintExpressionModel Expression { get; set; }
    }
}
