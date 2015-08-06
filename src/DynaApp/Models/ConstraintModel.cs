using System;

namespace DynaApp.Models
{
    [Serializable]
    public class ConstraintModel : ConnectableModel
    {
        public ConstraintModel(string constraintName, string rawExpression)
            : base(constraintName)
        {
            this.Expression = new ConstraintExpressionModel(rawExpression);
        }

        public ConstraintModel()
            : base("New constraint")
        {
            
        }

        public ConstraintExpressionModel Expression { get; set; }
    }
}
