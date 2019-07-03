using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    internal class OrangeBundleMap
    {
        private readonly List<OrangeSingletonVariableMap> _variableMap;

        internal OrangeBundleMap(BundleModel bundle)
        {
            Bundle = bundle;
            _variableMap = new List<OrangeSingletonVariableMap>();
        }

        internal BundleModel Bundle { get; }

        internal IReadOnlyCollection<OrangeSingletonVariableMap> GetVariableMaps()
        {
            return new ReadOnlyCollection<OrangeSingletonVariableMap>(_variableMap);
        }

        internal void Add(SingletonVariableModel singletonVariable, SolverVariable solverVariable)
        {
            _variableMap.Add(new OrangeSingletonVariableMap(singletonVariable, solverVariable));
        }

        internal OrangeSingletonVariableMap GetVariableByName(string variableName)
        {
            return _variableMap.FirstOrDefault(variableMap => variableMap.ModelVariable.Name.IsEqualTo(variableName));
        }
    }
}
