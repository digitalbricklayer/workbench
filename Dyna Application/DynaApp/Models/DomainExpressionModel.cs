using System;

namespace DynaApp.Models
{
    [Serializable]
    public class DomainExpressionModel
    {
        public DomainExpressionModel(string rawDomainExpression)
        {
            this.Expression = rawDomainExpression;
        }

        public DomainExpressionModel()
        {
            this.Expression = string.Empty;
        }

        public string Expression { get; set; }
    }
}