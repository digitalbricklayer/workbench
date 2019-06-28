using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// A set of values that may be assigned to one or more variables.
    /// </summary>
    internal sealed class ValueSet
    {
        private readonly List<Value> _values;

        internal ValueSet(IEnumerable<Value> variableValues)
        {
            Contract.Requires<ArgumentNullException>(variableValues != null);

            _values = new List<Value>(variableValues);
        }

        internal ValueSet(Value value)
        {
            _values = new List<Value> { value};
        }

        internal IEnumerable<Value> Values => new ReadOnlyCollection<Value>(_values);

        internal int GetAt(int index)
        {
            if (index < 0 || index >= _values.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _values[index].Content;
        }

        internal int GetValueFor(string variableName)
        {
            return _values.First(value => value.VariableName == variableName).Content;
        }

        internal bool IsAssigned(string variableName)
        {
            return _values.Any(value => value.VariableName == variableName);
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

        private bool IsAllUnique(IEnumerable<Value> variableValues)
        {
            var allVariableNames = new HashSet<string>();

            return variableValues.All(value => allVariableNames.Add(value.VariableName));
        }
    }
}
