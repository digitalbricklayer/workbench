using System;
using System.Diagnostics.Contracts;
using Google.OrTools.ConstraintSolver;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Convert the model representation into a representation usable 
    /// by the or-tools solver.
    /// </summary>
    class ModelConverter
    {
        private readonly Google.OrTools.ConstraintSolver.Solver solver;
        private ModelModel model;
        private readonly OrToolsCache cache;
        private AllDifferentConstraintConverter allDifferentConstraintConverter;
        private ExpressionConstraintConverter expressionConstraintConverter;

        /// <summary>
        /// Initialize the model converter with a Google or-tools solver.
        /// </summary>
        public ModelConverter(Google.OrTools.ConstraintSolver.Solver theSolver,
                              OrToolsCache theCache)
        {
            Contract.Requires<ArgumentNullException>(theSolver != null);
            this.solver = theSolver;
            this.cache = theCache;
        }

        /// <summary>
        /// Convert the model into a representation used by the Google or-tools solver.
        /// </summary>
        /// <param name="theModel">The model model.</param>
        public void ConvertFrom(ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            this.model = theModel;
            ProcessVariables(theModel);
            ProcessConstraints(theModel);
        }

        private void ProcessConstraints(ModelModel theModel)
        {
            this.allDifferentConstraintConverter = new AllDifferentConstraintConverter(this.solver, this.cache, this.model);
            foreach (var constraint in theModel.Constraints)
            {
                var expressionConstraint = constraint as ExpressionConstraintGraphicModel;
                if (expressionConstraint == null)
                {
                    this.allDifferentConstraintConverter.ProcessConstraint((AllDifferentConstraintGraphicModel)constraint);
                }
                else
                {
                    this.expressionConstraintConverter.ProcessConstraint(expressionConstraint);
                }
            }
        }

        private void ProcessVariables(ModelModel theModel)
        {
            this.expressionConstraintConverter = new ExpressionConstraintConverter(this.solver, this.cache, this.model);
            ProcessSingletonVariables(theModel);
            ProcessAggregateVariables(theModel);
        }

        private void ProcessAggregateVariables(ModelModel theModel)
        {
            foreach (var aggregate in theModel.Aggregates)
            {
                var theVariableBand = VariableBandEvaluator.GetVariableBand(aggregate.Variable);
                var theVariableRange = theVariableBand.GetRange();
                var orVariableVector = this.solver.MakeIntVarArray(aggregate.AggregateCount,
                                                                   theVariableRange.Lower,
                                                                   theVariableRange.Upper,
                                                                   aggregate.Name);
                this.cache.AddAggregate(aggregate.Name,
                                        new Tuple<AggregateVariableGraphicModel, IntVarVector>(aggregate, orVariableVector));
                foreach (var orVar in orVariableVector)
                {
                    this.cache.AddVariable(orVar);
                }
            }
        }

        private void ProcessSingletonVariables(ModelModel theModel)
        {
            foreach (var variable in theModel.Singletons)
            {
                var orVariable = ProcessVariable(variable);
                this.cache.AddSingleton(variable.Name, new Tuple<VariableGraphicModel, IntVar>(variable, orVariable));
            }
        }

        private IntVar ProcessVariable(VariableGraphicModel variable)
        {
            var theVariableBand = VariableBandEvaluator.GetVariableBand(variable.Variable);
            var theVariableRange = theVariableBand.GetRange();
            var orVariable = solver.MakeIntVar(theVariableRange.Lower, theVariableRange.Upper, variable.Name);
            this.cache.AddVariable(orVariable);

            return orVariable;
        }
    }
}
