using System;
using System.Diagnostics.Contracts;
using Google.OrTools.ConstraintSolver;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core
{
    public class ConstraintBuilderVisitor : IConstraintExpressionVisitor
    {
        private readonly Google.OrTools.ConstraintSolver.Solver solver;
        private WrappedConstraint outerConstraint;

        public ConstraintBuilderVisitor(Google.OrTools.ConstraintSolver.Solver theSolver)
        {
            Contract.Requires<ArgumentNullException>(theSolver != null);
            this.solver = theSolver;
        }

        public void Visit(AggregateVariableReferenceNode theNode)
        {
        }

        public void Visit(SingletonVariableReferenceNode theNode)
        {
        }

        public void Visit(BinaryExpressionNode theNode)
        {
        }

        public void Visit(SubscriptNode subscriptNode)
        {
        }

        public void Visit(ExpressionNode theNode)
        {
        }

        public void Visit(ConstraintExpressionNode theNode)
        {

            this.outerConstraint = new WrappedConstraint(null);
        }

        public void Visit(LiteralNode theNode)
        {
        }

        public void Visit(VariableNameNode theNode)
        {
        }

        public Constraint GetExpressionConstraint()
        {
            return null;
        }
    }
}
