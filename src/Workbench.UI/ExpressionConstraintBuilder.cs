using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench
{
    public class ExpressionConstraintBuilder
    {
        private ConstraintExpressionModel expression = new ConstraintExpressionModel();

        public ExpressionConstraintBuilder WithExpression(string theExpression)
        {
            this.expression = new ConstraintExpressionModel(theExpression);
            return this;
        }

        public ExpressionConstraintVisualizerViewModel Build()
        {
            var theConstraint = new ExpressionConstraintModel(new ModelName("New Constraint"), this.expression);
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
