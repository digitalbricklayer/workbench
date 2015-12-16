using System;

namespace Dyna.Core.Models
{
    /// <summary>
    /// Reference to an aggregate variable.
    /// </summary>
    [Serializable]
    public class AggregateVariableReference
    {
        public AggregateVariableReference(string newIdentifier, int aggregateIndex)
        {
            this.IdentifierName = newIdentifier;
            this.Index = aggregateIndex;
        }

        public AggregateVariableReference()
        {
            this.IdentifierName = String.Empty;
        }

        public int Index { get; private set; }

        public string IdentifierName { get; private set; }
    }
}