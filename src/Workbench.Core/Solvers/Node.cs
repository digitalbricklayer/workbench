namespace Workbench.Core.Solvers
{
    /// <summary>
    /// A node in an arc.
    /// </summary>
    internal sealed class Node
    {
        /// <summary>
        /// Initialize a node with an expression node.
        /// </summary>
        /// <param name="variable">Variable inside the node.</param>
        internal Node(IntegerVariable variable)
        {
            Variable = variable;
        }

        /// <summary>
        /// Gets the variable.
        /// </summary>
        internal IntegerVariable Variable { get; }
    }
}
