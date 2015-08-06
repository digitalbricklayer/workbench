using System;

namespace DynaApp.Models
{
    [Serializable]
    public class ConstraintExpressionModel
    {
        public ConstraintExpressionModel(string rawExpression)
        {
            this.Expression = rawExpression;
        }

        public ConstraintExpressionModel()
        {
            this.Expression = string.Empty;
        }

        public string Expression { get; set; }
    }
}