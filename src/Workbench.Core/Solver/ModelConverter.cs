using System;
using System.Diagnostics;
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
        private readonly AllDifferentConstraintConverter allDifferentConstraintConverter;
        private readonly ExpressionConstraintConverter expressionConstraintConverter;

        /// <summary>
        /// Initialize the model converter with a Google or-tools solver.
        /// </summary>
        public ModelConverter(Google.OrTools.ConstraintSolver.Solver theSolver,
                              OrToolsCache theCache)
        {
            Contract.Requires<ArgumentNullException>(theSolver != null);
            this.solver = theSolver;
            this.cache = theCache;
            this.allDifferentConstraintConverter = new AllDifferentConstraintConverter(theSolver, theCache);
            this.expressionConstraintConverter = new ExpressionConstraintConverter(theSolver, theCache);
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
            foreach (var constraint in theModel.Constraints)
            {
                var expressionConstraint = constraint as ExpressionConstraintModel;
                if (expressionConstraint == null)
                {
                    this.allDifferentConstraintConverter.ProcessConstraint((AllDifferentConstraintModel)constraint);
                }
                else
                {
                    this.expressionConstraintConverter.ProcessConstraint(expressionConstraint);
                }
            }
        }

        private void ProcessVariables(ModelModel theModel)
        {
            ProcessSingletonVariables(theModel);
            ProcessAggregateVariables(theModel);
        }

        private void ProcessAggregateVariables(ModelModel theModel)
        {
            foreach (var aggregate in theModel.Aggregates)
            {
                var band = this.GetVariableBand(aggregate);
                var orVariableVector = this.solver.MakeIntVarArray(aggregate.AggregateCount,
                                                                   band.Item1,
                                                                   band.Item2,
                                                                   aggregate.Name);
                this.cache.AddAggregate(aggregate.Name,
                                        new Tuple<AggregateVariableModel, IntVarVector>(aggregate, orVariableVector));
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
                this.cache.AddSingleton(variable.Name,
                                        new Tuple<VariableModel, IntVar>(variable, orVariable));
            }
        }

        private IntVar ProcessVariable(VariableModel variable)
        {
            var band = GetVariableBand(variable);
            var orVariable = solver.MakeIntVar(band.Item1, band.Item2, variable.Name);
            this.cache.AddVariable(orVariable);

            return orVariable;
        }

        private Tuple<long, long> GetVariableBand(VariableModel theVariable)
        {
            Debug.Assert(!theVariable.DomainExpression.IsEmpty);

            if (theVariable.DomainExpression.InlineDomain != null)
            {
                var inlineDomain = theVariable.DomainExpression.InlineDomain;
                return new Tuple<long, long>(inlineDomain.LowerBand,
                                             inlineDomain.UpperBand);
            }

            var sharedDomainName = theVariable.DomainExpression.DomainReference.DomainName;
            var sharedDomain = this.model.GetSharedDomainByName(sharedDomainName);
            return new Tuple<long, long>(sharedDomain.Expression.LowerBand,
                                         sharedDomain.Expression.UpperBand);
        }
    }
}
