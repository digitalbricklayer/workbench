using System;
using System.Diagnostics;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Fluent interface for building workspaces.
    /// </summary>
    public class WorkspaceContext
    {
        private readonly WorkspaceModel model;

        internal WorkspaceContext(WorkspaceModel theWorkspace)
        {
            if (theWorkspace == null)
                throw new ArgumentNullException("theWorkspace");
            this.model = theWorkspace;
        }

        public WorkspaceContext AddSingleton(string theVariableName, string theDomainExpression)
        {
            var newVariable = new VariableModel(theVariableName, theDomainExpression);
            this.model.Model.AddVariable(newVariable);

            return this;
        }

        public WorkspaceContext AddAggregate(string newAggregateName, int aggregateSize, string newDomainExpression)
        {
            var newVariable = new AggregateVariableModel(newAggregateName, aggregateSize, newDomainExpression);
            this.model.Model.AddVariable(newVariable);

            return this;
        }

        public WorkspaceContext WithSharedDomain(string newDomainName, string newDomainExpression)
        {
            var newDomain = new DomainModel(newDomainName, new Point(1, 1),  new DomainExpressionModel(newDomainExpression));
            this.model.Model.AddSharedDomain(newDomain);

            return this;
        }

        public WorkspaceContext WithSharedDomain(DomainModel theDomainExpression)
        {
            this.model.Model.AddSharedDomain(theDomainExpression);
            return this;
        }

        public WorkspaceContext WithConstraint(string theConstraintExpression)
        {
            this.model.Model.AddConstraint(new ConstraintModel(theConstraintExpression));
            return this;
        }

        public ModelModel GetModel()
        {
            Debug.Assert(this.model != null);
            return this.model.Model;
        }

        public DisplayModel GetDisplay()
        {
            Debug.Assert(this.model != null);
            return this.model.Display;
        }

        public WorkspaceModel Build()
        {
            return this.model;
        }
    }
}