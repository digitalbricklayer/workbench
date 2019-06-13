namespace Workbench.Core.Solvers
{
    /// <summary>
    /// A node participating in an arc.
    /// </summary>
    internal abstract class Node
    {
        /// <summary>
        /// Initialize a node with content.
        /// </summary>
        /// <param name="content">Content of the node.</param>
        internal Node(object content)
        {
            Content = content;
        }

        /// <summary>
        /// Gets the variable.
        /// </summary>
        internal object Content { get; }

        /// <summary>
        /// Is the node node consistent.
        /// </summary>
        /// <returns>True if the node is node consistent or False if the node is not node consistent.</returns>
        internal abstract bool IsNodeConsistent();
    }
}
