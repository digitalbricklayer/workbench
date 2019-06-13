using System.Text;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Value of a variable in the solver.
    /// </summary>
    internal sealed class Value
    {
        internal Value(string variableName, int value)
        {
            VariableName = variableName;
            Content = value;
        }

        internal string VariableName { get; }
        internal int Content { get; }

        public override string ToString()
        {
            var output = new StringBuilder();

            output.Append($"{VariableName}:{Content}");

            return output.ToString();
        }
    }
}
