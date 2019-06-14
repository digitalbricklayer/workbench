using System.Text;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Value of a variable in the solver.
    /// </summary>
    internal sealed class Value
    {
        internal Value(VariableBase variable, int value)
        {
            Variable = variable;
            Content = value;
        }

        internal VariableBase Variable { get; }
        internal string VariableName => Variable.Name;
        internal int Content { get; }

        public override string ToString()
        {
            var output = new StringBuilder();

            output.Append($"{VariableName}:{Content}");

            return output.ToString();
        }
    }
}
