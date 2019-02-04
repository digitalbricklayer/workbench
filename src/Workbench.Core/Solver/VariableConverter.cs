using Google.OrTools.ConstraintSolver;
using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Convert the model variables into a representation that the or-tools solver understands.
    /// </summary>
    internal class VariableConverter
    {
        private readonly Google.OrTools.ConstraintSolver.Solver solver;
        private readonly OrToolsCache cache;
        private readonly ValueMapper valueMapper;

        /// <summary>
        /// Initialize the variable converter with a Google or-tools solver, a or-tools cache and a solver / domain mapper.
        /// </summary>
        internal VariableConverter(Google.OrTools.ConstraintSolver.Solver theSolver, OrToolsCache theCache, ValueMapper theValueMapper)
        {
            Contract.Requires<ArgumentNullException>(theSolver != null);
            Contract.Requires<ArgumentNullException>(theCache != null);
            Contract.Requires<ArgumentNullException>(theValueMapper != null);

            this.solver = theSolver;
            this.cache = theCache;
            this.valueMapper = theValueMapper;
        }

        /// <summary>
        /// Convert all variables into a representation understood by the solver.
        /// </summary>
        /// <param name="theModel">The model.</param>
        internal void ConvertVariables(ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);

            ConvertSingletonVariables(theModel);
            ConvertAggregateVariables(theModel);
        }

        private void ConvertAggregateVariables(ModelModel theModel)
        {
            foreach (var aggregate in theModel.Aggregates)
            {
                var variableBand = VariableBandEvaluator.GetVariableBand(aggregate);
                this.valueMapper.AddVariableDomainValue(aggregate, variableBand);
                var variableRange = variableBand.GetRange();
                var orVariableVector = this.solver.MakeIntVarArray(aggregate.AggregateCount,
                                                                   variableRange.Lower,
                                                                   variableRange.Upper,
                                                                   aggregate.Name.Text);
                this.cache.AddAggregate(aggregate.Name.Text,
                                        new Tuple<AggregateVariableModel, IntVarVector>(aggregate, orVariableVector));
                foreach (var orVar in orVariableVector)
                {
                    this.cache.AddVariable(orVar);
                }
            }
        }

        private void ConvertSingletonVariables(ModelModel theModel)
        {
            foreach (var variable in theModel.Singletons)
            {
                var variableBand = VariableBandEvaluator.GetVariableBand(variable);
                this.valueMapper.AddVariableDomainValue(variable, variableBand);
                var orVariable = ProcessVariable(variable);
                this.cache.AddSingleton(variable.Name.Text, new Tuple<SingletonVariableModel, IntVar>(variable, orVariable));
            }
        }

        private IntVar ProcessVariable(VariableModel variable)
        {
            var variableBand = VariableBandEvaluator.GetVariableBand(variable);
            var variableRange = variableBand.GetRange();
            var orVariable = solver.MakeIntVar(variableRange.Lower, variableRange.Upper, variable.Name.Text);
            this.cache.AddVariable(orVariable);

            return orVariable;
        }
    }
}
