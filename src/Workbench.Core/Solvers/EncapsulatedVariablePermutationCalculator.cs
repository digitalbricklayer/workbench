using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    internal sealed class EncapsulatedVariablePermutationCalculator
    {
        private readonly OrangeModelSolverMap _modelSolverMap;

        internal EncapsulatedVariablePermutationCalculator(OrangeModelSolverMap modelSolverMap)
        {
            Contract.Requires<ArgumentNullException>(modelSolverMap != null);

            _modelSolverMap = modelSolverMap;
        }

        internal EncapsulatedVariableDomainValue Compute(TernaryConstraintExpression ternaryConstraint)
        {
            Contract.Requires<ArgumentNullException>(ternaryConstraint != null);

            var valueSetAccumulator = new List<ValueSet>();
            var leftSource = ternaryConstraint.GetLeftSource();
            var leftPossibleValues = leftSource.PossibleValues;
            var rightSource = ternaryConstraint.GetRightSource();
            var rightPossibleValues = new List<int>(rightSource.PossibleValues);

            foreach (var leftPossibleValue in leftPossibleValues)
            {
                foreach (var rightPossibleValue in rightPossibleValues)
                {
                    switch (ternaryConstraint.ExpressionNode.InnerExpression.Operator)
                    {
                        case OperatorType.Equals:
                            if (leftPossibleValue == rightPossibleValue)
                            {
                                var valueSet = new ValueSet(new[] { new Value(_modelSolverMap.GetSolverVariableByName(leftSource.VariableName), leftPossibleValue),
                                                                                 new Value(_modelSolverMap.GetSolverVariableByName(rightSource.VariableName), rightPossibleValue) });
                                valueSetAccumulator.Add(valueSet);
                            }
                            break;

                        case OperatorType.NotEqual:
                            if (leftPossibleValue != rightPossibleValue)
                            {
                                var valueSet = new ValueSet(new[] { new Value(_modelSolverMap.GetSolverVariableByName(leftSource.VariableName), leftPossibleValue),
                                                                                 new Value(_modelSolverMap.GetSolverVariableByName(rightSource.VariableName), rightPossibleValue) });
                                valueSetAccumulator.Add(valueSet);
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
