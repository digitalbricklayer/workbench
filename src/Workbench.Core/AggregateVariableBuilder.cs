using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core
{
    public sealed class AggregateVariableBuilder
    {
        private ModelName variableName;
        private ModelModel model;
        private int? size;
        private VariableDomainExpressionModel domain;

        public AggregateVariableBuilder WithName(string theVariableName)
        {
            this.variableName = new ModelName(theVariableName);
            return this;
        }

        public AggregateVariableBuilder WithDomain(string theExpression)
        {
            this.domain = new VariableDomainExpressionModel(theExpression);
            return this;
        }

        public AggregateVariableBuilder WithModel(ModelModel theModel)
        {
            this.model = theModel;
            return this;
        }

        public AggregateVariableBuilder WithSize(int theVariableSize)
        {
            this.size = theVariableSize;
            return this;
        }

        public AggregateVariableModel Build()
        {
            Contract.Assume(this.model != null);
            Contract.Assume(this.variableName != null);

            return new AggregateVariableModel(this.model.Workspace, this.variableName, GetSizeOrDefault(), GetExpressionOrDefault());
        }

        private VariableDomainExpressionModel GetExpressionOrDefault()
        {
            return this.domain ?? new VariableDomainExpressionModel();
        }

        private int GetSizeOrDefault()
        {
            return this.size ?? AggregateVariableModel.DefaultSize;
        }
    }
}
