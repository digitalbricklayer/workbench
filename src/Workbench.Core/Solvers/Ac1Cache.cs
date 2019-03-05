using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    internal class Ac1Cache
    {
        private readonly Dictionary<string, Tuple<SingletonVariableModel, IntegerVariable>> _singletonVariableMap;

        internal Ac1Cache()
        {
            _singletonVariableMap = new Dictionary<string, Tuple<SingletonVariableModel, IntegerVariable>>();
        }

        internal Dictionary<string, Tuple<SingletonVariableModel, IntegerVariable>> SingletonVariableMap => _singletonVariableMap;

        internal void AddSingleton(string name, Tuple<SingletonVariableModel, IntegerVariable> tuple)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(name));
            _singletonVariableMap.Add(name, tuple);
        }

        internal IntegerVariable GetVariableByName(string variableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(variableName));
            return _singletonVariableMap[variableName].Item2;
        }
    }
}