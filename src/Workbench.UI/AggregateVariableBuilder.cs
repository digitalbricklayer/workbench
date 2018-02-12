using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench
{
    public sealed class AggregateVariableBuilder
    {
        private ModelName variableName;
        private ModelModel model;
        private IEventAggregator eventAggregator;
        private IViewModelService viewModelService;
        private IDataService dataService;
        private int? size;
        private VariableDomainExpressionModel domain;

        public AggregateVariableBuilder WithName(string theVariableName)
        {
            this.variableName = new ModelName(theVariableName);
            return this;
        }

        public AggregateVariableBuilder WithDomain(string theExpression)
        {
            this.domain = new VariableDomainExpressionModel(theExpression);
            return this;
        }

        public AggregateVariableBuilder WithEventAggregator(IEventAggregator theEventAggregator)
        {
            this.eventAggregator = theEventAggregator;
            return this;
        }

        public AggregateVariableBuilder WithViewModelService(IViewModelService theViewModelService)
        {
            this.viewModelService = theViewModelService;
            return this;
        }

        public AggregateVariableBuilder WithDataService(IDataService theDataService)
        {
            this.dataService = theDataService;
            return this;
        }

        public AggregateVariableBuilder WithModel(ModelModel theModel)
        {
            this.model = theModel;
            return this;
        }

        public AggregateVariableBuilder WithSize(int theVariableSize)
        {
            this.size = theVariableSize;
            return this;
        }

        public AggregateVariableVisualizerViewModel Build()
        {
            var theSingletonVariable = new AggregateVariableModel(this.model, this.variableName, GetSizeOrDefault(), GetExpressionOrDefault());
            return new AggregateVariableVisualizerViewModel(theSingletonVariable,
                                                            new AggregateVariableEditorViewModel(new AggregateVariableGraphicModel(theSingletonVariable), GetEventAggregatorOrDefault(), GetDataServiceOrDefault(), GetViewModelServiceOrDefault()),
                                                            new AggregateVariableViewerViewModel(new AggregateVariableGraphicModel(theSingletonVariable)));
        }

        private VariableDomainExpressionModel GetExpressionOrDefault()
        {
            return this.domain ?? new VariableDomainExpressionModel();
        }

        private int GetSizeOrDefault()
        {
            return this.size ?? 1;
        }

        private IViewModelService GetViewModelServiceOrDefault()
        {
            return this.viewModelService ?? new DefaultViewModelService();
        }

        private IDataService GetDataServiceOrDefault()
        {
            return this.dataService ?? new DefaultDataService();
        }

        private IEventAggregator GetEventAggregatorOrDefault()
        {
            return this.eventAggregator ?? new EventAggregator();
        }
    }
}
