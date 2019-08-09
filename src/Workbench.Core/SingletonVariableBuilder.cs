using System.Diagnostics;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core
{
    public sealed class SingletonVariableBuilder
    {
        private ModelName variableName;
        private ModelModel model;
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

        public SingletonVariableBuilder Inside(ModelModel theModel)
        {
            this.model = theModel;
            return this;
        }

        public SingletonVariableModel Build()
        {
            Debug.Assert(this.model != null);
            Debug.Assert(this.variableName != null);

            return new SingletonVariableModel(this.model, this.variableName, GetDomainOrDefault());
        }

        private InlineDomainModel GetDomainOrDefault()
        {
            return this.domain ?? new InlineDomainModel();
        }
    }
}
