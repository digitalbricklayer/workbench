namespace Workbench.Core.Solvers
{
    internal sealed class TernaryConstraintExpressionSolution
    {
        internal TernaryConstraintExpressionSolution(TernaryConstraintExpression ternaryConstraintExpression, EncapsulatedVariableDomainValue encapsulatedVariableDomainValue)
        {
            Expression = ternaryConstraintExpression;
            DomainValue = encapsulatedVariableDomainValue;
        }

        internal TernaryConstraintExpression Expression { get; }
        internal EncapsulatedVariableDomainValue DomainValue { get; }
    }
}