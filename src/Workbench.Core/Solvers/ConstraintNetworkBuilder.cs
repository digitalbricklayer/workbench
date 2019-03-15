using System;
using System.Linq;
using Workbench.Core.Models;
using Workbench.Core.Repeaters;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Build the constraint network from the model.
    /// </summary>
    internal sealed class ConstraintNetworkBuilder
    {
        private readonly OrangeCache _cache;
        private ConstraintNetwork _constraintNetwork;
        private readonly ValueMapper _valueMapper = new ValueMapper();

        /// <summary>
        /// Initialize a constraint network builder with a cache.
        /// </summary>
        /// <param name="cache">Cache to track model elements to solver equivalents.</param>
        internal ConstraintNetworkBuilder(OrangeCache cache)
        {
            _cache = cache;
        }

        internal ConstraintNetwork Build(ModelModel model)
        {
            CacheVariables(model);
            _constraintNetwork = CreateConstraintNetwork();
            PopulateConstraintNetwork(model);
            CreateVariables(model);

            return _constraintNetwork;
        }

        private ConstraintNetwork CreateConstraintNetwork()
        {
            return new ConstraintNetwork();
        }

        private void PopulateConstraintNetwork(ModelModel model)
        {
            foreach (var constraint in model.Constraints)
            {
                switch (constraint)
                {
                    case ExpressionConstraintModel expressionConstraint:
                        CreateArcFrom(expressionConstraint);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void CreateArcFrom(ExpressionConstraintModel constraint)
        {
            if (constraint.Expression.Node.HasExpander)
            {
                var repeater = new OrangeConstraintRepeater(_constraintNetwork, _cache, constraint.Parent, _valueMapper);
                repeater.Process(repeater.CreateContextFrom(constraint));
            }
            else
            {
                _constraintNetwork.AddArc(new ArcBuilder(_cache).Build(constraint));
            }
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
                                            new OrangeSingletonVariableMap(singletonVariable, integerVariable));
                        break;

                    case AggregateVariableModel aggregateVariable:
                        var aggregateIntegerVariable = CreateIntegerVariableFrom(aggregateVariable);
                        _cache.AddAggregate(aggregateVariable.Name,
                                            new OrangeAggregateVariableMap(aggregateVariable, aggregateIntegerVariable));
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