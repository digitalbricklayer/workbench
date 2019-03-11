using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Google.OrTools.ConstraintSolver;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    internal class OrBundleMap
    {
        private readonly List<OrSingletonVariableMap> _variableMap;

        internal OrBundleMap(BundleModel bundle)
        {
            Bundle = bundle;
            _variableMap = new List<OrSingletonVariableMap>();
        }

        internal BundleModel Bundle { get; }

        internal IReadOnlyCollection<OrSingletonVariableMap> GetVariableMaps()
        {
            return new ReadOnlyCollection<OrSingletonVariableMap>(_variableMap);
        }

        internal void Add(SingletonVariableModel singletonVariable, IntVar orVariable)
        {
            _variableMap.Add(new OrSingletonVariableMap(singletonVariable, orVariable));
        }

        internal OrSingletonVariableMap GetVariableByName(string variableName)
        {
            return _variableMap.FirstOrDefault(variableMap => variableMap.ModelVariable.Name.IsEqualTo(variableName));
        }
    }
}