using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
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
        /// Build an arc from an expression constraint.
        /// </summary>
        /// <param name="expressionConstraintNode">Expression constraint node.</param>
        /// <returns>Arc</returns>
        internal Arc Build(ConstraintExpressionNode expressionConstraintNode)
        {
            return new Arc(new Node(expressionConstraintNode.InnerExpression.LeftExpression),
                           new Node(expressionConstraintNode.InnerExpression.RightExpression), 
                           CreateConnectorFrom(CreateConstraintFrom(expressionConstraintNode)));
        }

        private ConstraintExpression CreateConstraintFrom(ConstraintExpressionNode binaryExpressionNode)
        {
            return new ConstraintExpression(binaryExpressionNode);
        }

        private NodeConnector CreateConnectorFrom(ConstraintExpression constraint)
        {
            return new NodeConnector(constraint);
        }
    }
}
