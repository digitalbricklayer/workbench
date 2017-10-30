using System;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Reference to an aggregate variable.
    /// </summary>
    [Serializable]
    public class AggregateVariableReferenceModel
    {
        public AggregateVariableReferenceModel(string newIdentifier, int aggregateIndex)
        {
            this.IdentifierName = newIdentifier;
            this.Index = aggregateIndex;
        }

        public AggregateVariableReferenceModel()
        {
            this.IdentifierName = String.Empty;
        }

        public int Index { get; private set; }

        public string IdentifierName { get; private set; }
    }
}