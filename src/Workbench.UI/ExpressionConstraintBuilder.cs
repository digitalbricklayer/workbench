using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench
{
    public class ExpressionConstraintBuilder
    {
        private ConstraintExpressionModel expression = new ConstraintExpressionModel();
        private ModelName name = new ModelName("New Constraint");

        public ExpressionConstraintBuilder WithName(string theName)
        {
            this.name = new ModelName(theName);
            return this;
        }

        public ExpressionConstraintBuilder WithExpression(string theExpression)
        {
            this.expression = new ConstraintExpressionModel(theExpression);
            return this;
        }

        public ExpressionConstraintVisualizerViewModel Build()
        {
            var theConstraint = new ExpressionConstraintModel(this.name, this.expression);
            var theConstraintGraphic = new ExpressionConstraintGraphicModel(theConstraint);
            return new ExpressionConstraintVisualizerViewModel(theConstraint,
                                                               new ExpressionConstraintEditorViewModel(theConstraintGraphic, GetEventAggregatorOrDefault(), GetDataServiceOrDefault(), GetViewModelServiceOrDefault()),
                                                               new ExpressionConstraintViewerViewModel(theConstraintGraphic));
        }

        private IDataService GetDataServiceOrDefault()
        {
            return new DefaultDataService();
        }

        private IEventAggregator GetEventAggregatorOrDefault()
        {
            return new EventAggregator();
        }

        private IViewModelService GetViewModelServiceOrDefault()
        {
            return new DefaultViewModelService();
        }
    }
}
