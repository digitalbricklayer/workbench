using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench
{
    public class AllDifferentConstraintBuilder
    {
        private AllDifferentConstraintExpressionModel expression = new AllDifferentConstraintExpressionModel();
        private ModelName name = new ModelName("New Constraint");

        public AllDifferentConstraintBuilder WithName(string theName)
        {
            this.name = new ModelName(theName);
            return this;
        }

        public AllDifferentConstraintBuilder WithExpression(string theExpression)
        {
            this.expression = new AllDifferentConstraintExpressionModel(theExpression);
            return this;
        }

        public AllDifferentConstraintVisualizerViewModel Build()
        {
            var theConstraint = new AllDifferentConstraintModel(this.name, this.expression);
            var theConstraintGraphic = new AllDifferentConstraintGraphicModel(theConstraint);
            return new AllDifferentConstraintVisualizerViewModel(theConstraint,
                                                               new AllDifferentConstraintEditorViewModel(theConstraintGraphic, GetEventAggregatorOrDefault(), GetDataServiceOrDefault(), GetViewModelServiceOrDefault()),
                                                               new AllDifferentConstraintViewerViewModel(theConstraintGraphic));
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
