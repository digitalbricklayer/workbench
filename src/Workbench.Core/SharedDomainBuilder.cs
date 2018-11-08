using System;
using Workbench.Core.Models;

namespace Workbench.Core
{
    public class SharedDomainBuilder
    {
        private ModelName domainName;
        private SharedDomainExpressionModel expression;
        private ModelModel _model;

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

        public SharedDomainBuilder Inside(ModelModel theModel)
        {
            _model = theModel;
            return this;
        }

        public SharedDomainModel Build()
        {
            if (_model == null) throw new Exception("A model must be provided when building a shared domain.");
            return new SharedDomainModel(_model, GetNameOrDefault(), GetExpressionOrDefault());
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
