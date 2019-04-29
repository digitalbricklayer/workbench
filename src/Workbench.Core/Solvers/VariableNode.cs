namespace Workbench.Core.Solvers
{
    /// <summary>
    /// A node containing a variable.
    /// </summary>
    internal sealed class VariableNode : Node
    {
        public VariableNode(SolverVariable variable)
            : base(variable)
        {
            Variable = variable;
        }

        /// <summary>
        /// Gets the variable.
        /// </summary>
        internal SolverVariable Variable { get; }

        /// <summary>
        /// Is the node node consistent.
        /// </summary>
        /// <returns>True if the node is node consistent or False if the node is not node consistent.</returns>
        internal override bool IsNodeConsistent()
        {
            return !Variable.Domain.IsEmpty;
        }
    }
}