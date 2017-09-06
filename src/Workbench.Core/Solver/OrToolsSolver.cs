using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Google.OrTools.ConstraintSolver;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Constraint solver implemented using Google or-tools library.
    /// </summary>
    public class OrToolsSolver : IDisposable
    {
        private Google.OrTools.ConstraintSolver.Solver solver;
        private ModelConverter modelConverter;
        private readonly OrToolsCache cache = new OrToolsCache();

        /// <summary>
        /// Solve the problem in the model.
        /// </summary>
        /// <param name="theModel">The model to solve.</param>
        public SolveResult Solve(ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);

            var validateContext = new ModelValidationContext();
            if (!theModel.Validate(validateContext)) return SolveResult.InvalidModel;

            this.solver = new Google.OrTools.ConstraintSolver.Solver(theModel.Name);

            this.modelConverter = new ModelConverter(this.solver, this.cache);
            modelConverter.ConvertFrom(theModel);

            // Search
            var decisionBuilder = solver.MakePhase(this.cache.Variables,
                                                   Google.OrTools.ConstraintSolver.Solver.CHOOSE_FIRST_UNBOUND,
                                                   Google.OrTools.ConstraintSolver.Solver.INT_VALUE_DEFAULT);
            var collector = CreateCollector();
            var solveResult = this.solver.Solve(decisionBuilder, collector);
            if (!solveResult) return SolveResult.Failed;

            var theSolutionSnapshot = ExtractValuesFrom(collector);
            theSolutionSnapshot.Duration = TimeSpan.FromMilliseconds(this.solver.WallTime());
            return new SolveResult(SolveStatus.Success, theSolutionSnapshot);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.solver == null) return;
            this.solver.Dispose();
            this.solver = null;
        }

        private SolutionSnapshot ExtractValuesFrom(SolutionCollector solutionCollector)
        {
            var theSnapshot = new SolutionSnapshot();
            foreach (var variableTuple in this.cache.SingletonVariableMap)
            {
                var boundValue = solutionCollector.Value(0, variableTuple.Value.Item2);
                var newValue = new ValueModel(variableTuple.Value.Item1, Convert.ToInt32(boundValue));
                theSnapshot.AddSingletonValue(newValue);
            }

            foreach (var aggregateTuple in this.cache.AggregateVariableMap)
            {
                var newValues = new List<int>();
                var orVariables = aggregateTuple.Value.Item2;
                foreach (var orVariable in orVariables)
                {
                    var boundValue = solutionCollector.Value(0, orVariable);
                    newValues.Add(Convert.ToInt32(boundValue));
                }
                var newValue = new ValueModel(aggregateTuple.Value.Item1, newValues);
                theSnapshot.AddAggregateValue(newValue);
            }

            return theSnapshot;
        }

        private SolutionCollector CreateCollector()
        {
            var collector = this.solver.MakeFirstSolutionCollector();
            foreach (var variableTuple in this.cache.SingletonVariableMap)
                collector.Add(variableTuple.Value.Item2);
            foreach (var variableTuple in this.cache.AggregateVariableMap)
            {
                var variablesInsideAggregate = variableTuple.Value.Item2;
                foreach (var intVar in variablesInsideAggregate)
                    collector.Add(intVar);
            }

            return collector;
        }
    }
}
