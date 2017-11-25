using System;
using System.Collections.Generic;
using Google.OrTools.ConstraintSolver;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Cache references between items in Google or-tools and their 
    /// equivalents in the model.
    /// </summary>
    internal class OrToolsCache
    {
        private readonly Dictionary<string, Tuple<SingletonVariableGraphicModel, IntVar>> singletonVariableMap;
        private readonly Dictionary<string, Tuple<AggregateVariableGraphicModel, IntVarVector>> aggregateVariableMap;

        internal OrToolsCache()
        {
            this.singletonVariableMap = new Dictionary<string, Tuple<SingletonVariableGraphicModel, IntVar>>();
            this.aggregateVariableMap = new Dictionary<string, Tuple<AggregateVariableGraphicModel, IntVarVector>>();
            this.Variables = new IntVarVector();
        }

        internal Dictionary<string, Tuple<SingletonVariableGraphicModel, IntVar>> SingletonVariableMap => this.singletonVariableMap;
        internal Dictionary<string, Tuple<AggregateVariableGraphicModel, IntVarVector>> AggregateVariableMap => this.aggregateVariableMap;
        internal IntVarVector Variables { get; private set; }

        internal void AddAggregate(string name, Tuple<AggregateVariableGraphicModel, IntVarVector> tuple)
        {
            this.aggregateVariableMap.Add(name, tuple);
        }

        internal void AddSingleton(string name, Tuple<SingletonVariableGraphicModel, IntVar> tuple)
        {
            this.singletonVariableMap.Add(name, tuple);
        }

        internal IntVarVector GetVectorByName(string aggregateName)
        {
            return this.aggregateVariableMap[aggregateName].Item2;
        }

        internal IntVar GetSingletonVariableByName(string theVariableName)
        {
            return this.singletonVariableMap[theVariableName].Item2;
        }

        internal IntVar GetAggregateVariableByName(string theVariableName, int index)
        {
            var orVariables = this.aggregateVariableMap[theVariableName].Item2;
            return orVariables[index];
        }

        internal void AddVariable(IntVar orVar)
        {
            Variables.Add(orVar);
        }
    }
}
