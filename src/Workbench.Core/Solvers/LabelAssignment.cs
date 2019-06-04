namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Captures a variable assignment state.     
    /// </summary>
    internal sealed class LabelAssignment
    {
        internal LabelAssignment(SolverVariable variable, int value)
        {
            Variable = variable;
            Value = value;
        }

        internal SolverVariable Variable { get; }
        internal int Value { get; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"<{Variable},{Value}>";
        }
    }
}
