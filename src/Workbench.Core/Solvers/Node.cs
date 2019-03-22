namespace Workbench.Core.Solvers
{
    internal abstract class Node
    {
        /// <summary>
        /// Gets whether the node has been solved.
        /// </summary>
        /// <returns>True if the node is solved, false if the node is not solved.</returns>
        internal abstract bool IsSolved();
    }
}
