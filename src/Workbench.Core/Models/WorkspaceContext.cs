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
        private readonly WorkspaceModel workspace;

        internal WorkspaceContext(WorkspaceModel theWorkspace)
        {
            if (theWorkspace == null)
                throw new ArgumentNullException("theWorkspace");
            this.workspace = theWorkspace;
        }

        public WorkspaceContext AddSingleton(string theVariableName, string theDomainExpression)
        {
            var newVariable = new VariableModel(theVariableName, theDomainExpression);
            this.workspace.Model.AddVariable(newVariable);

            return this;
        }

        public WorkspaceContext AddAggregate(string newAggregateName, int aggregateSize, string newDomainExpression)
        {
            var newVariable = new AggregateVariableModel(newAggregateName, aggregateSize, newDomainExpression);
            this.workspace.Model.AddVariable(newVariable);

            return this;
        }

        public WorkspaceContext WithSharedDomain(string newDomainName, string newDomainExpression)
        {
            var newDomain = new DomainModel(newDomainName, new Point(1, 1),  new DomainExpressionModel(newDomainExpression));
            this.workspace.Model.AddSharedDomain(newDomain);

            return this;
        }

        public WorkspaceContext WithSharedDomain(DomainModel theDomainExpression)
        {
            this.workspace.Model.AddSharedDomain(theDomainExpression);
            return this;
        }

        public WorkspaceContext WithConstraint(string theConstraintExpression)
        {
            this.workspace.Model.AddConstraint(new ConstraintModel(theConstraintExpression));
            return this;
        }

        public WorkspaceContext WithVisualizerBindingTo(string variableNameToBindTo)
        {
            var theVisualizer = new VariableVisualizerModel(new Point());
            theVisualizer.BindTo(this.workspace.Model.GetVariableByName(variableNameToBindTo));
            this.workspace.AddVisualizer(theVisualizer);
            return this;
        }

        public ModelModel GetModel()
        {
            Debug.Assert(this.workspace != null);
            return this.workspace.Model;
        }

        public DisplayModel GetDisplay()
        {
            Debug.Assert(this.workspace != null);
            return this.workspace.Solution.Display;
        }

        public WorkspaceModel Build()
        {
            return this.workspace;
        }
    }
}