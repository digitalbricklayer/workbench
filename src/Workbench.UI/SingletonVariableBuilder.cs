using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench
{
    public sealed class SingletonVariableBuilder
    {
        private ModelName variableName;
        private ModelModel model;
        private VariableDomainExpressionModel domain;

        public SingletonVariableBuilder WithName(string theVariableName)
        {
            this.variableName = new ModelName(theVariableName);
            return this;
        }

        public SingletonVariableBuilder WithDomain(string theExpression)
        {
            this.domain = new VariableDomainExpressionModel(theExpression);
            return this;
        }

        public SingletonVariableBuilder WithModel(ModelModel theModel)
        {
            this.model = theModel;
            return this;
        }

        public SingletonVariableModel Build()
        {
            Contract.Assume(this.model != null);
            Contract.Assume(this.variableName != null);

            return new SingletonVariableModel(this.model, this.variableName, GetExpressionOrDefault());
        }

        private VariableDomainExpressionModel GetExpressionOrDefault()
        {
            return this.domain ?? new VariableDomainExpressionModel();
        }
    }
}
