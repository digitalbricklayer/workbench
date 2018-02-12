using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench
{
    public class DomainBuilder
    {
        private ModelName domainName;
        private DomainExpressionModel expression;

        public DomainBuilder WithName(string theVariableName)
        {
            this.domainName = new ModelName(theVariableName);
            return this;
        }

        public DomainBuilder WithDomain(string theExpression)
        {
            this.expression = new DomainExpressionModel(theExpression);
            return this;
        }

        public DomainVisualizerViewModel Build()
        {
            var theSingletonVariable = new DomainModel(GetNameOrDefault(), GetExpressionOrDefault());
            return new DomainVisualizerViewModel(theSingletonVariable,
                new DomainEditorViewModel(new DomainGraphicModel(theSingletonVariable), GetEventAggregatorOrDefault(), GetDataServiceOrDefault(), GetViewModelServiceOrDefault()),
                new DomainViewerViewModel(new DomainGraphicModel(theSingletonVariable)));
        }

        private ModelName GetNameOrDefault()
        {
            return this.domainName ?? new ModelName();
        }

        private DomainExpressionModel GetExpressionOrDefault()
        {
            return this.expression ?? new DomainExpressionModel();
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
