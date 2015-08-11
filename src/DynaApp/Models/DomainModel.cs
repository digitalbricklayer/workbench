using System;

namespace DynaApp.Models
{
    [Serializable]
    public class DomainModel : GraphicModel
    {
        public DomainModel(string domainName, string rawDomainExpression)
            : base(domainName)
        {
            this.Expression = new DomainExpressionModel(rawDomainExpression);
        }

        public DomainModel()
            : base("New domain")
        {
            
        }

        public DomainExpressionModel Expression { get; set; }
    }
}
