using Google.OrTools.ConstraintSolver;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using System;
using System.Collections.Generic;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Extract the snapshot from the or-tools solution.
    /// </summary>
    internal class SnapshotExtractor
    {
        private OrToolsCache orToolsCache;
        private ValueMapper valueMapper;
        private SolutionSnapshot snapshot;

        /// <summary>
        /// Initialize a snapshot extractor with a or-tools cache and a value mapper.
        /// </summary>
        /// <param name="theOrToolsCache">Or-tools cache.</param>
        /// <param name="theValueMapper">Value mapper between domain and solver values.</param>
        internal SnapshotExtractor(OrToolsCache theOrToolsCache, ValueMapper theValueMapper)
        {
            Contract.Requires<ArgumentNullException>(theOrToolsCache != null);
            Contract.Requires<ArgumentNullException>(theValueMapper != null);

            this.orToolsCache = theOrToolsCache;
            this.valueMapper = theValueMapper;
            this.snapshot = new SolutionSnapshot();
        }

        /// <summary>
        /// Extract the snapshot from the solution collector.
        /// </summary>
        /// <param name="theSolutionCollector">Or-tools solution collector.</param>
        /// <returns>Solution snapshot model.</returns>
        internal SolutionSnapshot ExtractValuesFrom(SolutionCollector theSolutionCollector)
        {
            Contract.Requires<ArgumentNullException>(theSolutionCollector != null);

            ExtractSingletonValuesFrom(theSolutionCollector);
            ExtractAggregateValuesFrom(theSolutionCollector);

            return this.snapshot;
        }

        private void ExtractAggregateValuesFrom(SolutionCollector theSolutionCollector)
        {
            foreach (var aggregateTuple in this.orToolsCache.AggregateVariableMap)
            {
                var newValueBindings = new List<ValueModel>();
                var orVariables = aggregateTuple.Value.Item2;
                var variableCounter = 1;
                foreach (var orVariable in orVariables)
                {
                    var solverValue = theSolutionCollector.Value(0, orVariable);
                    var modelValue = ConvertSolverValueToModel(aggregateTuple.Value.Item1, variableCounter, solverValue);
                    newValueBindings.Add(new ValueModel(modelValue));
                    variableCounter++;
                }
                var newCompoundLabel = new CompoundLabelModel(aggregateTuple.Value.Item1, newValueBindings);
                this.snapshot.AddAggregateValue(newCompoundLabel);
            }
        }

        private void ExtractSingletonValuesFrom(SolutionCollector theSolutionCollector)
        {
            foreach (var variableTuple in this.orToolsCache.SingletonVariableMap)
            {
                var solverValue = theSolutionCollector.Value(0, variableTuple.Value.Item2);
                var modelValue = ConvertSolverValueToModel(variableTuple.Value.Item1, solverValue);
                var newValue = new SingletonLabelModel(variableTuple.Value.Item1, new ValueModel(modelValue));
                this.snapshot.AddSingletonValue(newValue);
            }
        }

        private object ConvertSolverValueToModel(SingletonVariableModel theVariable, long solverValue)
        {
            var theVariableDomainValue = this.valueMapper.GetDomainValueFor(theVariable);
            return theVariableDomainValue.MapFrom(solverValue);
        }

        private object ConvertSolverValueToModel(AggregateVariableModel theVariable, int index, long solverValue)
        {
            var theVariableDomainValue = this.valueMapper.GetDomainValueFor(theVariable);
            return theVariableDomainValue.MapFrom(solverValue);
        }
    }
}
