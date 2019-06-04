using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// A set of values that may be assigned to one or more variables.
    /// </summary>
    internal sealed class ValueSet
    {
        private readonly List<int> _values;

        internal ValueSet(IEnumerable<int> variableValues)
        {
            Contract.Requires<ArgumentNullException>(variableValues != null);

            _values = new List<int>(variableValues);
        }

        internal int GetAt(int index)
        {
            if (index < 0 || index >= _values.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _values[index];
        }

        public override string ToString()
        {
            var output = new StringBuilder();
            output.Append('(');

            var i = 1;
            foreach (var value in _values)
            {
                output.Append(value);
                if (i < _values.Count)
                    output.Append(",");
                i++;
            }

            output.Append(')');

            return output.ToString();
        }

    }
}