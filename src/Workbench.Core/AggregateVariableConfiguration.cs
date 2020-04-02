using System;
using Workbench.Core.Models;

namespace Workbench.Core
{
    public sealed class AggregateVariableConfiguration
    {
        private string _name;
        private int _size;
        private string _domainExpression;
        private readonly WorkspaceModel _workspace;

        public AggregateVariableConfiguration(WorkspaceModel workspace)
        {
            _workspace = workspace;
        }

        public void WithName(string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
                throw new ArgumentException(nameof(variableName));
            _name = variableName;
        }

        public void WithSize(int variableSize)
        {
            if (variableSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(variableSize));
            _size = variableSize;
        }

        public void WithDomain(string domainExpression)
        {
            if (string.IsNullOrWhiteSpace(domainExpression))
                throw new ArgumentException(nameof(domainExpression));
            _domainExpression = domainExpression;
        }

        public AggregateVariableModel Build()
        {
            return new AggregateVariableModel(_workspace.Model, new ModelName(_name), _size, new InlineDomainModel(_domainExpression));
        }
    }
}
