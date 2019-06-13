using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Build one or more arcs from an expression constraint node.
    /// </summary>
    internal sealed class ArcBuilder
    {
        private readonly OrangeModelSolverMap _modelSolverMap;

        /// <summary>
        /// Initialize an arc builder with a model solver map.
        /// </summary>
        /// <param name="modelSolverMap">Map between solver and model entities.</param>
        internal ArcBuilder(OrangeModelSolverMap modelSolverMap)
        {
            _modelSolverMap = modelSolverMap;
        }

        /// <summary>
        /// Build one or more arcs as necessary to represent the expression constraint.
        /// </summary>
        /// <param name="expressionConstraintNode">Expression constraint node.</param>
        /// <returns>One or more arcs</returns>
        internal IReadOnlyCollection<Arc> Build(ConstraintExpressionNode expressionConstraintNode)
        {
            switch (expressionConstraintNode.InnerExpression.Operator)
            {
                // Ternary operators
                case OperatorType.Equals:
                case OperatorType.NotEqual:
                    return BuildTernaryExpression(expressionConstraintNode);

                // Binary operators
                default:
                    return BuildBinaryExpression(expressionConstraintNode);
            }
        }

        /// <summary>
        /// Build one or more arcs from the ternary constraint expression.
        /// </summary>
        /// <remarks>
        /// There will be more than one arc because binarization of the expression requires introducing an
        /// encapsulated variable between the two variables involved in the expression.
        /// </remarks>
        /// <param name="expressionConstraintNode">Expression constraint abstract syntax tree.</param>
        /// <returns>One or more arcs.</returns>
        private IReadOnlyCollection<Arc> BuildTernaryExpression(ConstraintExpressionNode expressionConstraintNode)
        {
            var arcAccumulator = new List<Arc>();
            var left = CreateNodeFrom(expressionConstraintNode.InnerExpression.LeftExpression);
            var right = CreateNodeFrom(expressionConstraintNode.InnerExpression.RightExpression);
            var encapsulatedVariable = new EncapsulatedVariable("U");
            var encapsulatedVariableNode = new EncapsulatedVariableNode(encapsulatedVariable);
            var arc1 = new Arc(left, encapsulatedVariableNode, new EncapsulatedVariableConnector(left, encapsulatedVariableNode, new EncapsulatedSelector(1)));
            arcAccumulator.Add(arc1);
            var arc2 = new Arc(right, encapsulatedVariableNode, new EncapsulatedVariableConnector(encapsulatedVariableNode, right, new EncapsulatedSelector(2)));
            arcAccumulator.Add(arc2);
            var ternaryConstraintExpression = new TernaryConstraintExpression(expressionConstraintNode, encapsulatedVariableNode, arc1, arc2);
            var encapsulatedVariableDomainValue = ComputeEncapsulatedDomain(ternaryConstraintExpression);
            encapsulatedVariable.DomainValue = encapsulatedVariableDomainValue;
            var expressionSolution = new TernaryConstraintExpressionSolution(ternaryConstraintExpression, encapsulatedVariableDomainValue);
            _modelSolverMap.AddTernaryExpressionSolution(expressionSolution);

            return new ReadOnlyCollection<Arc>(arcAccumulator);
        }

        private IReadOnlyCollection<Arc> BuildBinaryExpression(ConstraintExpressionNode expressionConstraintNode)
        {
            var left = CreateNodeFrom(expressionConstraintNode.InnerExpression.LeftExpression);
            if (!expressionConstraintNode.InnerExpression.RightExpression.IsLiteral)
            {
                var right = CreateNodeFrom(expressionConstraintNode.InnerExpression.RightExpression);
                return new ReadOnlyCollection<Arc>(new[] {new Arc(left, right, CreateConnectorFrom(left, right, CreateConstraintFrom(expressionConstraintNode)))});
            }

            return new ReadOnlyCollection<Arc>(new[] {new Arc(left, left, CreateConnectorFrom(left, left, CreateConstraintFrom(expressionConstraintNode)))});
        }

        private Node CreateNodeFrom(ExpressionNode expressionConstraintNode)
        {
            var variable = ExtractVariableFrom(expressionConstraintNode);
            var existingNode = _modelSolverMap.GetNodeByName(variable.Name);

            return existingNode ?? new VariableNode(variable);
        }

        private SolverVariable ExtractVariableFrom(ExpressionNode expressionConstraintNode)
        {
            switch (expressionConstraintNode.InnerExpression)
            {
                case SingletonVariableReferenceNode singletonVariableReference:
                    return _modelSolverMap.GetSolverSingletonVariableByName(singletonVariableReference.VariableName);

                case AggregateVariableReferenceNode aggregateVariableReference:
                    return _modelSolverMap.GetSolverAggregateVariableByName(aggregateVariableReference.VariableName,
                                                                            aggregateVariableReference.SubscriptStatement.Subscript);

                case SingletonVariableReferenceExpressionNode singletonVariableExpression:
                    return _modelSolverMap.GetSolverSingletonVariableByName(singletonVariableExpression.VariableReference.VariableName);

                case AggregateVariableReferenceExpressionNode aggregateVariableExpression:
                    return _modelSolverMap.GetSolverAggregateVariableByName(aggregateVariableExpression.VariableReference.VariableName,
                                                                            aggregateVariableExpression.VariableReference.SubscriptStatement.Subscript);

                default:
                    throw new NotImplementedException();
            }
        }

        private BinaryConstraintExpression CreateConstraintFrom(ConstraintExpressionNode binaryExpressionNode)
        {
            return new BinaryConstraintExpression(binaryExpressionNode);
        }

        private NodeConnector CreateConnectorFrom(Node left, Node right, BinaryConstraintExpression constraint)
        {
            return new ConstraintExpressionConnector(left, right, constraint);
        }

        private static EncapsulatedVariableDomainValue ComputeEncapsulatedDomain(TernaryConstraintExpression ternaryConstraintExpression)
        {
            var encapsulatedVariableCalculator = new EncapsulatedVariablePermutationCalculator(ternaryConstraintExpression);
            return encapsulatedVariableCalculator.Compute();
        }
    }
}
