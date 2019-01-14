using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core
{
    public sealed class AggregateVariableBuilder
    {
        private ModelName variableName;
        private BundleModel bundle;
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

        public AggregateVariableBuilder Inside(BundleModel theBundle)
        {
            this.bundle = theBundle;
            return this;
        }

        public AggregateVariableBuilder WithSize(int theVariableSize)
        {
            this.size = theVariableSize;
            return this;
        }

        public AggregateVariableModel Build()
        {
            Contract.Assume(this.bundle != null);
            Contract.Assume(this.variableName != null);

            return new AggregateVariableModel(this.bundle.Workspace, this.variableName, GetSizeOrDefault(), GetDomainOrDefault());
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
