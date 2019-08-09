using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    internal sealed class EncapsulatedVariablePermutationCalculator
    {
        private readonly OrangeModelSolverMap _modelSolverMap;
        private readonly OrangeValueMapper _valueMapper;

        internal EncapsulatedVariablePermutationCalculator(OrangeModelSolverMap modelSolverMap, OrangeValueMapper valueMapper)
        {
            _modelSolverMap = modelSolverMap;
            _valueMapper = valueMapper;
        }

        internal EncapsulatedVariableDomainValue Compute(TernaryConstraintExpression ternaryConstraint)
        {
            var valueSetAccumulator = new List<ValueSet>();
            var leftSource = ternaryConstraint.GetLeftSource();
            var leftPossibleValues = leftSource.PossibleValues;
            var expression = ternaryConstraint.ExpressionNode;
            IReadOnlyCollection<int> rightPossibleValues;
            if (!expression.InnerExpression.RightExpression.IsLiteral)
            {
                var rightSource = ternaryConstraint.GetRightSource();
                rightPossibleValues = new ReadOnlyCollection<int>(rightSource.PossibleValues.ToList());
            }
            else
            {
                var lhsVariable = _modelSolverMap.GetModelVariableByName(leftSource.VariableName);
                var range = _valueMapper.GetDomainValueFor(lhsVariable);
                var modelValue = expression.InnerExpression.RightExpression.GetLiteral();
                var solverValue = range.MapTo(modelValue);
                rightPossibleValues = new ReadOnlyCollection<int>(new List<int> { solverValue });
            }

            foreach (var leftPossibleValue in leftPossibleValues)
            {
                foreach (var rightPossibleValue in rightPossibleValues)
                {
                    switch (ternaryConstraint.ExpressionNode.InnerExpression.Operator)
                    {
                        case OperatorType.Equals:
                            if (leftPossibleValue == rightPossibleValue)
                            {
                                if (!expression.InnerExpression.RightExpression.IsLiteral)
                                {
                                    var rightSource = ternaryConstraint.GetRightSource();
                                    var valueSet = new ValueSet(new[] { new Value(_modelSolverMap.GetSolverVariableByName(leftSource.VariableName), leftPossibleValue),
                                                                new Value(_modelSolverMap.GetSolverVariableByName(rightSource.VariableName), rightPossibleValue) });
                                    valueSetAccumulator.Add(valueSet);
                                }
                                else
                                {
                                    var valueSet = new ValueSet(new[] { new Value(_modelSolverMap.GetSolverVariableByName(leftSource.VariableName), leftPossibleValue) });
                                    valueSetAccumulator.Add(valueSet);
                                }
                            }
                            break;

                        case OperatorType.NotEqual:
                            if (leftPossibleValue != rightPossibleValue)
                            {
                                if (!expression.InnerExpression.RightExpression.IsLiteral)
                                {
                                    var rightSource = ternaryConstraint.GetRightSource();
                                    var valueSet = new ValueSet(new[] { new Value(_modelSolverMap.GetSolverVariableByName(leftSource.VariableName), leftPossibleValue),
                                                                new Value(_modelSolverMap.GetSolverVariableByName(rightSource.VariableName), rightPossibleValue) });
                                    valueSetAccumulator.Add(valueSet);
                                }
                                else
                                {
                                    var valueSet = new ValueSet(new[] { new Value(_modelSolverMap.GetSolverVariableByName(leftSource.VariableName), leftPossibleValue) });
                                    valueSetAccumulator.Add(valueSet);
                                }
                            }
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }
            }

            return new EncapsulatedVariableDomainValue(ternaryConstraint.EncapsulatedVariable, valueSetAccumulator);
        }
    }

    internal sealed class Source
    {
        internal Source(string variableName, IEnumerable<int> possibleValues)
        {
            VariableName = variableName;
            PossibleValues = possibleValues;
        }

        internal string VariableName { get; }
        internal IEnumerable<int> PossibleValues { get; }
    }
}
