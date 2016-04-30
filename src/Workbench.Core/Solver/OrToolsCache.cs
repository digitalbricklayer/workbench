using System;
using System.Collections.Generic;
using Google.OrTools.ConstraintSolver;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Cache references between items in Google or-tools and their 
    /// equivalents in the Model.
    /// </summary>
    class OrToolsCache
    {
        private readonly Dictionary<string, Tuple<VariableModel, IntVar>> singletonVariableMap;
        private readonly Dictionary<string, Tuple<AggregateVariableModel, IntVarVector>> aggregateVariableMap;

        public OrToolsCache()
        {
            this.singletonVariableMap = new Dictionary<string, Tuple<VariableModel, IntVar>>();
            this.aggregateVariableMap = new Dictionary<string, Tuple<AggregateVariableModel, IntVarVector>>();
            this.Variables = new IntVarVector();
        }

        public Dictionary<string, Tuple<VariableModel, IntVar>> SingletonVariableMap => this.singletonVariableMap;
        public Dictionary<string, Tuple<AggregateVariableModel, IntVarVector>> AggregateVariableMap => this.aggregateVariableMap;
        public IntVarVector Variables { get; private set; }

        public void AddAggregate(string name, Tuple<AggregateVariableModel, IntVarVector> tuple)
        {
            this.aggregateVariableMap.Add(name, tuple);
        }

        public void AddSingleton(string name, Tuple<VariableModel, IntVar> tuple)
        {
            this.singletonVariableMap.Add(name, tuple);
        }

        public IntVarVector GetVectorByName(string aggregateName)
        {
            return this.aggregateVariableMap[aggregateName].Item2;
        }

        public IntVar GetSingletonVariableByName(string theVariableName)
        {
            return this.singletonVariableMap[theVariableName].Item2;
        }

        public IntVar GetAggregateVariableByName(string theVariableName, int index)
        {
            var orVariables = this.aggregateVariableMap[theVariableName].Item2;
            return orVariables[index - 1];
        }

        public void AddVariable(IntVar orVar)
        {
            this.Variables.Add(orVar);
        }
    }
}
