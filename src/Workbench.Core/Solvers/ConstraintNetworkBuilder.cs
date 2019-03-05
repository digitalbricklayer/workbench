using System;
using System.Linq;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    internal sealed class ConstraintNetworkBuilder
    {
        private readonly Ac1Cache _cache;

        internal ConstraintNetworkBuilder(Ac1Cache cache)
        {
            _cache = cache;
        }

        internal ConstraintNetwork Build(ModelModel model)
        {
            CreateVariables(model);
            return CreateConstraintNetwork(model);
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

        private void CreateVariables(ModelModel model)
        {
            foreach (var variable in model.Variables)
            {
                switch (variable)
                {
                    case SingletonVariableModel singletonVariable:
                        _cache.AddSingleton(singletonVariable.Name,
                                            new Tuple<SingletonVariableModel, IntegerVariable>(singletonVariable, CreateIntegerVariableFrom(singletonVariable)));
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
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