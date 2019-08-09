using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Workbench.Core.Models;
using Workbench.Core.Parsers;
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
            _arcBuilder = new ArcBuilder(_modelSolverMap, valueMapper);
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
            ConvertBuckets(model);
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
						
					case AllDifferentConstraintModel allDifferentConstraint:
						CreateArcFrom(allDifferentConstraint);
						break;

                    default:
                        throw new NotImplementedException();
                }
            }

            // Constraints inside bundles must be processed after the bucket maps have been created
            foreach (var bucket in model.Buckets)
            {
                foreach (var allDifferentConstraint in bucket.Bundle.AllDifferentConstraints)
                {
                    var variableNames = new List<string>(ExtractVariablesFrom(allDifferentConstraint.Expression.Text));

                    for (var bundleCounter = 0; bundleCounter < bucket.Size; bundleCounter++)
                    {
                        var bucketName = bucket.Name.Text;
                        var expressionText = $"%{bucketName}[{bundleCounter}].{variableNames[0]} <> %{bucketName}[{bundleCounter}].{variableNames[1]}";
                        var interpreter = new ConstraintExpressionParser();
                        var parseResult = interpreter.Parse(expressionText);
                        Debug.Assert(parseResult.IsSuccess);
                        _constraintNetwork.AddArc(_arcBuilder.Build(parseResult.Root));
                    }
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

        private void CreateArcFrom(AllDifferentConstraintModel constraint)
        {
            var solverAggregateVariable = _modelSolverMap.GetSolverAggregateVariableByName(constraint.Expression.Text);

            /*
             * Iterate through all variables inside the aggregate and ensure
             * that each variable is not equal to all variables to their
             * right. The not equal operator is commutative so there is no need
             * to express the constraint the other way around.
             */
            for (var i = 0; i < solverAggregateVariable.Variables.Count; i++)
            {
                for (var z = i + 1; z < solverAggregateVariable.Variables.Count; z++)
                {
                    var expressionText = $"${solverAggregateVariable.Name}[{i}] <> ${solverAggregateVariable.Name}[{z}]";
                    var interpreter = new ConstraintExpressionParser();
                    var parseResult = interpreter.Parse(expressionText);
                    Debug.Assert(parseResult.IsSuccess);
                    _constraintNetwork.AddArc(_arcBuilder.Build(parseResult.Root));
                }
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
                        var singletonSolverVariable = _modelSolverMap.GetSolverSingletonVariableByName(singletonVariable.Name);
                        _constraintNetwork.AddVariable(singletonSolverVariable);
                        break;

                    case AggregateVariableModel aggregateVariable:
                        var aggregateSolverVariable = _modelSolverMap.GetSolverAggregateVariableByName(aggregateVariable.Name);
                        _constraintNetwork.AddVariable(aggregateSolverVariable);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            foreach (var bucketVariable in model.Buckets)
            {
                var bucketVariableMap = _modelSolverMap.GetBucketVariableMapByName(bucketVariable.Name);
                _constraintNetwork.AddVariable(bucketVariableMap);
            }
        }

        /// <summary>
        /// Convert all buckets into a representation understood by the solver.
        /// </summary>
        /// <param name="model">The model.</param>
        internal void ConvertBuckets(ModelModel model)
        {
            foreach (var bucket in model.Buckets)
            {
                var bucketMap = ConvertBucket(bucket);
                _modelSolverMap.AddBucket(bucket.Name, bucketMap);
            }
        }

        private OrangeBucketVariableMap ConvertBucket(BucketVariableModel bucket)
        {
            var bucketMap = new OrangeBucketVariableMap(bucket);
            for (var i = 0; i < bucket.Size; i++)
            {
                var bundleMap = ConvertBundle(bucket, i);

                bucketMap.Add(bundleMap);
            }

            return bucketMap;
        }

        private OrangeBundleMap ConvertBundle(BucketVariableModel bucket, int bucketCount)
        {
            var bundleMap = new OrangeBundleMap(bucket.Bundle);
            foreach (var singleton in bucket.Bundle.Singletons)
            {
                var variableBand = VariableBandEvaluator.GetVariableBand(singleton);
                _valueMapper.AddBucketDomainValue(bucket, variableBand);
                var solverVariableName = CreateVariableNameFrom(bucket, bucketCount, singleton);
                var variableRange = variableBand.GetRange();
                var solverVariable = new SolverVariable(solverVariableName, CreateRangeFrom(variableRange));
                bundleMap.Add(singleton, solverVariable);
            }

            return bundleMap;
        }

        private IEnumerable<string> ExtractVariablesFrom(string expressionText)
        {
            var x = Array.ConvertAll(expressionText.Split(','), variableName => variableName.Trim());
            Debug.Assert(x.Length > 0);

            return x;
        }

        private static string CreateVariableNameFrom(BucketVariableModel bucket, int index, SingletonVariableModel singletonVariable)
        {
            return bucket.Name + "_" + Convert.ToString(index) + "_" + singletonVariable.Name;
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
            return CreateRangeFrom(variableRange);
        }

        private DomainRange CreateRangeFrom(Range variableRange)
        {
            var range = Enumerable.Range(Convert.ToInt32(variableRange.Lower),
                                         Convert.ToInt32(variableRange.Count)).ToArray();
            return new DomainRange(range);
        }
    }
}