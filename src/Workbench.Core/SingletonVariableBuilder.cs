using System.Diagnostics;
using Workbench.Core.Models;

namespace Workbench.Core
{
    public sealed class SingletonVariableBuilder
    {
        private ModelName variableName;
        private BundleModel bundle;
        private InlineDomainModel domain;

        public SingletonVariableBuilder WithName(string theVariableName)
        {
            this.variableName = new ModelName(theVariableName);
            return this;
        }

        public SingletonVariableBuilder WithDomain(string theExpression)
        {
            this.domain = new InlineDomainModel(theExpression);
            return this;
        }

        public SingletonVariableBuilder Inside(BundleModel theBundle)
        {
            this.bundle = theBundle;
            return this;
        }

        public SingletonVariableModel Build()
        {
            Debug.Assert(this.bundle != null);
            Debug.Assert(this.variableName != null);

            return new SingletonVariableModel(this.bundle, this.variableName, GetDomainOrDefault());
        }

        private InlineDomainModel GetDomainOrDefault()
        {
            return this.domain ?? new InlineDomainModel();
        }
    }
}
