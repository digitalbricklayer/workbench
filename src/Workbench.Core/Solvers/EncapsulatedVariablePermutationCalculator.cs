using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    internal sealed class EncapsulatedVariablePermutationCalculator
    {
        private readonly TernaryConstraintExpression _ternaryConstraint;

        internal EncapsulatedVariablePermutationCalculator(TernaryConstraintExpression ternaryConstraint)
        {
            Contract.Requires<ArgumentNullException>(ternaryConstraint != null);

            _ternaryConstraint = ternaryConstraint;
        }

        internal EncapsulatedVariableDomainValue Compute()
        {
            var valueSetAccumulator = new List<ValueSet>();
            var leftPossibleValues = _ternaryConstraint.GetLeftSource();
            var rightPossibleValues = new List<int>(_ternaryConstraint.GetRightSource());

            foreach (var leftPossibleValue in leftPossibleValues)
            {
                foreach (var rightPossibleValue in rightPossibleValues)
                {
                    switch (_ternaryConstraint.ExpressionNode.InnerExpression.Operator)
                    {
                        case OperatorType.Equals:
                            if (leftPossibleValue == rightPossibleValue)
                            {
                                var valueSet = new ValueSet(new[] { leftPossibleValue, rightPossibleValue });
                                valueSetAccumulator.Add(valueSet);
                            }
                            break;

                        case OperatorType.NotEqual:
                            if (leftPossibleValue != rightPossibleValue)
                            {
                                var valueSet = new ValueSet(new[] { leftPossibleValue, rightPossibleValue });
                                valueSetAccumulator.Add(valueSet);
                            }
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }
            }

            return new EncapsulatedVariableDomainValue(_ternaryConstraint.EncapsulatedVariable, valueSetAccumulator);
        }
    }
}
