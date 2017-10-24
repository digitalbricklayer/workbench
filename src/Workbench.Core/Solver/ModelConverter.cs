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
                return EvaluateInlineDomainExpression(theVariable);
            }

            return EvaluateSharedDomainExpression(theVariable);
        }

        /// <summary>
        /// The domain is specified by a domain expression inline with the variable
        /// </summary>
        /// <param name="theVariable">Variable with domain expression.</param>
        /// <returns>Tuple with upper and lower band.</returns>
        private Tuple<long, long> EvaluateInlineDomainExpression(VariableModel theVariable)
        {
            var inlineDomain = theVariable.DomainExpression.InlineDomain;
            var evaluatorContext = new DomainExpressionEvaluatorContext(inlineDomain, this.model);
            return DomainExpressionEvaluator.Evaluate(evaluatorContext);
        }

        ///<summary>
        /// The domain is specified in a shared domain seperately and the
        /// inline expression references the shared domain
        ///</summary>
        /// <param name="theVariable">Variable with domain expression.</param>
        /// <returns>Tuple with upper and lower band.</returns>
        private Tuple<long, long> EvaluateSharedDomainExpression(VariableModel theVariable)
        {
            var evaluatorContext = new SharedDomainExpressionEvaluatorContext(theVariable.DomainExpression.Node, this.model);
            return SharedDomainExpressionEvaluator.Evaluate(evaluatorContext);
        }
    }
}
