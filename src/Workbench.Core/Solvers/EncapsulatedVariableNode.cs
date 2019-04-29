namespace Workbench.Core.Solvers
{
    /// <summary>
    /// A node containing an encapsulated variable.
    /// </summary>
    internal sealed class EncapsulatedVariableNode : Node
    {
        internal EncapsulatedVariableNode(EncapsulatedVariable variable)
            : base(variable)
        {
            Variable = variable;
        }

        /// <summary>
        /// Gets the variable.
        /// </summary>
        internal EncapsulatedVariable Variable { get; }

        /// <summary>
        /// Is the node node consistent.
        /// </summary>
        /// <returns>True if the node is node consistent or False if the node is not node consistent.</returns>
        internal override bool IsNodeConsistent()
        {
#if false
            return !Variable.DomainValue.IsEmpty;
#else
            return true;
#endif
        }
    }
}
