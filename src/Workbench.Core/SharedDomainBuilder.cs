using Workbench.Core.Models;

namespace Workbench.Core
{
    public class SharedDomainBuilder
    {
        private ModelName domainName;
        private SharedDomainExpressionModel expression;

        public SharedDomainBuilder WithName(string theVariableName)
        {
            this.domainName = new ModelName(theVariableName);
            return this;
        }

        public SharedDomainBuilder WithDomain(string theExpression)
        {
            this.expression = new SharedDomainExpressionModel(theExpression);
            return this;
        }

        public SharedDomainModel Build()
        {
            return new SharedDomainModel(GetNameOrDefault(), GetExpressionOrDefault());
        }

        private ModelName GetNameOrDefault()
        {
            return this.domainName ?? new ModelName();
        }

        private SharedDomainExpressionModel GetExpressionOrDefault()
        {
            return this.expression ?? new SharedDomainExpressionModel();
        }
    }
}
