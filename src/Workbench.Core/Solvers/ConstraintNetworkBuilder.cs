using System;
using System.Linq;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Build the constraint network from the model.
    /// </summary>
    internal sealed class ConstraintNetworkBuilder
    {
        private readonly Ac1Cache _cache;
        private ConstraintNetwork _constraintNetwork;

        /// <summary>
        /// Initialize a constraint network builder with a cache.
        /// </summary>
        /// <param name="cache">Cache to track model elements to solver equivalents.</param>
        internal ConstraintNetworkBuilder(Ac1Cache cache)
        {
            _cache = cache;
        }

        internal ConstraintNetwork Build(ModelModel model)
        {
            CacheVariables(model);
            _constraintNetwork = CreateConstraintNetwork(model);
            CreateVariables(model);

            return _constraintNetwork;
        }

        private ConstraintNetwork CreateConstraintNetwork(ModelModel model)
        {
            var constraintNetwork = new ConstraintNetwork();
            foreach (var constraint in model.Constraints)
            {
                switch (constraint)
                {
                    case ExpressionConstraintModel expressionConstraint:
                        constraintNetwork.AddArc(new ArcBuilder(_cache).Build(expressionConstraint));
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            return constraintNetwork;
        }

        private void CacheVariables(ModelModel model)
        {
            foreach (var variable in model.Variables)
            {
                switch (variable)
                {
                    case SingletonVariableModel singletonVariable:
                        var integerVariable = CreateIntegerVariableFrom(singletonVariable);
                        _cache.AddSingleton(singletonVariable.Name,
                                            new Ac1SingletonVariableMap(singletonVariable, integerVariable));
                        break;

                    case AggregateVariableModel aggregateVariable:
                        var aggregateIntegerVariable = CreateIntegerVariableFrom(aggregateVariable);
                        _cache.AddAggregate(aggregateVariable.Name,
                                            new Ac1AggregateVariableMap(aggregateVariable, aggregateIntegerVariable));
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void CreateVariables(ModelModel model)
        {
            foreach (var variable in model.Variables)
            {
                switch (variable)
                {
                    case SingletonVariableModel singletonVariable:
                        var integerVariable = _cache.GetSolverSingletonVariableByName(singletonVariable.Name);
                        _constraintNetwork.AddVariable(integerVariable);
                        break;

                    case AggregateVariableModel aggregateVariable:
                        var aggregateIntegerVariable = _cache.GetSolverAggregateVariableByName(aggregateVariable.Name);
                        _constraintNetwork.AddVariable(aggregateIntegerVariable);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private AggregateIntegerVariable CreateIntegerVariableFrom(AggregateVariableModel aggregateVariable)
        {
            return new AggregateIntegerVariable(aggregateVariable.Name, aggregateVariable.AggregateCount, CreateRangeFrom(aggregateVariable));
        }

        private IntegerVariable CreateIntegerVariableFrom(SingletonVariableModel singletonVariable)
        {
            return new IntegerVariable(singletonVariable.Name, CreateRangeFrom(singletonVariable));
        }

        private DomainRange CreateRangeFrom(VariableModel variable)
        {
            var variableBand = VariableBandEvaluator.GetVariableBand(variable);
            var variableRange = variableBand.GetRange();
            var range = Enumerable.Range(Convert.ToInt32(variableRange.Lower),
                                         Convert.ToInt32(variableRange.Count)).ToArray();
            return new DomainRange(range);
        }
    }
}