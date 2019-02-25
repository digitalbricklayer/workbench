using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Google.OrTools.ConstraintSolver;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    internal class BundleMap
    {
        private readonly List<SingletonVariableMap> _variableMap;

        internal BundleMap(BundleModel bundle)
        {
            Bundle = bundle;
            _variableMap = new List<SingletonVariableMap>();
        }

        internal BundleModel Bundle { get; }

        internal IReadOnlyCollection<SingletonVariableMap> GetVariableMaps()
        {
            return new ReadOnlyCollection<SingletonVariableMap>(_variableMap);
        }

        internal void Add(SingletonVariableModel singletonVariable, IntVar orVariable)
        {
            _variableMap.Add(new SingletonVariableMap(singletonVariable, orVariable));
        }

        internal SingletonVariableMap GetVariableByName(string variableName)
        {
            return _variableMap.FirstOrDefault(variableMap => variableMap.ModelVariable.Name.IsEqualTo(variableName));
        }
    }
}