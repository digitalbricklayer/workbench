namespace Workbench.Core.Solvers
{
    internal sealed class EncapsulatedVariableConnector : NodeConnector
    {
        /// <summary>
        /// Initialize a connector with a constraint.
        /// </summary>
        /// <param name="left">Left node.</param>
        /// <param name="right">Right node.</param>
        /// <param name="selector">Index into the values.</param>
        internal EncapsulatedVariableConnector(Node left, Node right, EncapsulatedSelector selector)
            : base(left, right, selector)
        {
            Selector = selector;
        }

        /// <summary>
        /// Gets the constraint.
        /// </summary>
        internal EncapsulatedSelector Selector { get; }
    }
}
