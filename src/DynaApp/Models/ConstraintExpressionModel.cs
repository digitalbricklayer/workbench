using System;

namespace DynaApp.Models
{
    [Serializable]
    public class ConstraintExpressionModel
    {
        public ConstraintExpressionModel(string rawExpression)
        {
            this.Text = rawExpression;
        }

        public ConstraintExpressionModel()
        {
            this.Text = string.Empty;
        }

        public string Text { get; set; }
    }
}