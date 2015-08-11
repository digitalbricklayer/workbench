using System;

namespace DynaApp.Models
{
    [Serializable]
    public class ConstraintModel : GraphicModel
    {
        public ConstraintModel(string constraintName, string rawExpression)
            : base(constraintName)
        {
            this.Expression = new ConstraintExpressionModel(rawExpression);
        }

        public ConstraintModel()
            : base("New constraint")
        {
            this.Expression = new ConstraintExpressionModel();
        }

        public ConstraintExpressionModel Expression { get; set; }
    }
}
