using System.Collections.Generic;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// An encapsulated variable is used in binarization of ternary
    /// expressions i.e. introducing a variable with 2 new constraints.
    /// </summary>
    internal sealed class EncapsulatedVariable : VariableBase
    {
        /// <summary>
        /// Initialize an encapsulated variable with a variable name.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        internal EncapsulatedVariable(string variableName)
            : base(variableName)
        {
        }

        internal EncapsulatedVariableDomainValue DomainValue { get; set; }

        internal IEnumerable<ValueSet> GetCandidates()
        {
            return DomainValue.GetSets();
        }
    }
}
