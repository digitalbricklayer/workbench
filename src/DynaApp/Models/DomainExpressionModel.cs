using System;

namespace DynaApp.Models
{
    [Serializable]
    public class DomainExpressionModel
    {
        public DomainExpressionModel(string rawDomainExpression)
        {
            this.Text = rawDomainExpression;
        }

        public DomainExpressionModel()
        {
            this.Text = string.Empty;
        }

        public string Text { get; set; }
    }
}