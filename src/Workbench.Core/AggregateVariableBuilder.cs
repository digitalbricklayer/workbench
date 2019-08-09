using System.Diagnostics;
using Workbench.Core.Models;

namespace Workbench.Core
{
    public sealed class AggregateVariableBuilder
    {
        private ModelName variableName;
        private ModelModel model;
        private int? size;
        private InlineDomainModel domain;

        public AggregateVariableBuilder WithName(string theVariableName)
        {
            this.variableName = new ModelName(theVariableName);
            return this;
        }

        public AggregateVariableBuilder WithDomain(string theExpression)
        {
            this.domain = new InlineDomainModel(theExpression);
            return this;
        }

        public AggregateVariableBuilder Inside(ModelModel theModel)
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
            Debug.Assert(this.model != null);
            Debug.Assert(this.variableName != null);

            return new AggregateVariableModel(this.model.Workspace, this.variableName, GetSizeOrDefault(), GetDomainOrDefault());
        }

        private InlineDomainModel GetDomainOrDefault()
        {
            return this.domain ?? new InlineDomainModel();
        }

        private int GetSizeOrDefault()
        {
            return this.size ?? AggregateVariableModel.DefaultSize;
        }
    }
}
