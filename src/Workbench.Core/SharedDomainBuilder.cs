using System;
using Workbench.Core.Models;

namespace Workbench.Core
{
    public class SharedDomainBuilder
    {
        private ModelName domainName;
        private SharedDomainExpressionModel expression;
        private BundleModel _bundle;

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

        public SharedDomainBuilder Inside(BundleModel theModel)
        {
            _bundle = theModel;
            return this;
        }

        public SharedDomainModel Build()
        {
            if (_bundle == null) throw new Exception("A model must be provided when building a shared domain.");
            return new SharedDomainModel(_bundle, GetNameOrDefault(), GetExpressionOrDefault());
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
