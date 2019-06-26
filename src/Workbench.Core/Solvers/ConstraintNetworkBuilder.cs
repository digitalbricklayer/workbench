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
        private readonly OrangeModelSolverMap _modelSolverMap;
        private ConstraintNetwork _constraintNetwork;
        private readonly OrangeValueMapper _valueMapper;
        private readonly ArcBuilder _arcBuilder;

        /// <summary>
        /// Initialize a constraint network builder with a cache.
        /// </summary>
        /// <param name="modelSolverMap">Cache to track model elements to solver equivalents.</param>
        /// <param name="valueMapper">Solver to domain value mapper.</param>
        internal ConstraintNetworkBuilder(OrangeModelSolverMap modelSolverMap, OrangeValueMapper valueMapper)
        {
            _modelSolverMap = modelSolverMap;
            _valueMapper = valueMapper;
            _arcBuilder = new ArcBuilder(_modelSolverMap);
        }

        /// <summary>
        /// Build a constraint network from the model.
        /// </summary>
        /// <param name="model">Constraint model.</param>
        /// <returns>Constraint network.</returns>
        internal ConstraintNetwork Build(ModelModel model)
        {
            MapVariables(model);
            MapValues(model);
            _constraintNetwork = new ConstraintNetwork();
            PopulateConstraintNetwork(model);
            CreateVariables(model);

            return _constraintNetwork;
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
                // Process the constraint repeater adding arcs as necessary
                var repeater = new OrangeConstraintRepeater(_constraintNetwork, _modelSolverMap, constraint.Parent, _valueMapper);
                repeater.Process(repeater.CreateContextFrom(constraint));
            }
            else
            {
                _constraintNetwork.AddArc(_arcBuilder.Build(constraint.Expression.Node));
            }
        }

        private void MapVariables(ModelModel model)
        {
            foreach (var variable in model.Variables)
            {
                switch (variable)
                {
                    case SingletonVariableModel singletonVariable:
                        var integerVariable = CreateIntegerVariableFrom(singletonVariable);
                        _modelSolverMap.AddSingleton(singletonVariable.Name,
                                                     new OrangeSingletonVariableMap(singletonVariable, integerVariable));
                        break;

                    case AggregateVariableModel aggregateVariable:
                        var aggregateIntegerVariable = CreateIntegerVariableFrom(aggregateVariable);
                        _modelSolverMap.AddAggregate(aggregateVariable.Name,
                                                     new OrangeAggregateVariableMap(aggregateVariable, aggregateIntegerVariable));
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void MapValues(ModelModel model)
        {
            foreach (var variable in model.Variables)
            {
                switch (variable)
                {
                    case SingletonVariableModel singletonVariable:
                        var singletonVariableBand = VariableBandEvaluator.GetVariableBand(variable);
                        _valueMapper.AddVariableDomainValue(singletonVariable, singletonVariableBand);
                        break;

                    case AggregateVariableModel aggregateVariable:
                        var aggregateVariableBand = VariableBandEvaluator.GetVariableBand(aggregateVariable);
                        _valueMapper.AddVariableDomainValue(aggregateVariable, aggregateVariableBand);
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
                        var integerVariable = _modelSolverMap.GetSolverSingletonVariableByName(singletonVariable.Name);
                        _constraintNetwork.AddVariable(integerVariable);
                        break;

                    case AggregateVariableModel aggregateVariable:
                        var aggregateIntegerVariable = _modelSolverMap.GetSolverAggregateVariableByName(aggregateVariable.Name);
                        _constraintNetwork.AddVariable(aggregateIntegerVariable);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private AggregateSolverVariable CreateIntegerVariableFrom(AggregateVariableModel aggregateVariable)
        {
            return new AggregateSolverVariable(aggregateVariable.Name, aggregateVariable.AggregateCount, CreateRangeFrom(aggregateVariable));
        }

        private SolverVariable CreateIntegerVariableFrom(SingletonVariableModel singletonVariable)
        {
            return new SolverVariable(singletonVariable.Name, CreateRangeFrom(singletonVariable));
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