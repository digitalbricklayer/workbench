using Workbench.Core.Models;

namespace Workbench
{
    public class DomainBuilder
    {
        private ModelName domainName;
        private DomainExpressionModel expression;

        public DomainBuilder WithName(string theVariableName)
        {
            this.domainName = new ModelName(theVariableName);
            return this;
        }

        public DomainBuilder WithDomain(string theExpression)
        {
            this.expression = new DomainExpressionModel(theExpression);
            return this;
        }

        public DomainModel Build()
        {
            return new DomainModel(GetNameOrDefault(), GetExpressionOrDefault());
        }

        private ModelName GetNameOrDefault()
        {
            return this.domainName ?? new ModelName();
        }

        private DomainExpressionModel GetExpressionOrDefault()
        {
            return this.expression ?? new DomainExpressionModel();
        }
    }
}
