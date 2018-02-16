using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench
{
    public sealed class SingletonVariableBuilder
    {
        private ModelName variableName;
        private ModelModel model;
        private IEventAggregator eventAggregator;
        private IViewModelService viewModelService;
        private IDataService dataService;
        private VariableDomainExpressionModel domain;

        public SingletonVariableBuilder WithName(string theVariableName)
        {
            this.variableName = new ModelName(theVariableName);
            return this;
        }

        public SingletonVariableBuilder WithDomain(string theExpression)
        {
            this.domain = new VariableDomainExpressionModel(theExpression);
            return this;
        }

        public SingletonVariableBuilder WithEventAggregator(IEventAggregator theEventAggregator)
        {
            this.eventAggregator = theEventAggregator;
            return this;
        }

        public SingletonVariableBuilder WithViewModelService(IViewModelService theViewModelService)
        {
            this.viewModelService = theViewModelService;
            return this;
        }

        public SingletonVariableBuilder WithDataService(IDataService theDataService)
        {
            this.dataService = theDataService;
            return this;
        }

        public SingletonVariableBuilder WithModel(ModelModel theModel)
        {
            this.model = theModel;
            return this;
        }

        public SingletonVariableVisualizerViewModel Build()
        {
            Contract.Assume(this.model != null);
            Contract.Assume(this.variableName != null);

            var theAggregateVariable = new SingletonVariableModel(this.model, this.variableName, GetExpressionOrDefault());
            return new SingletonVariableVisualizerViewModel(theAggregateVariable,
                                                            new SingletonVariableEditorViewModel(new SingletonVariableGraphicModel(theAggregateVariable), GetEventAggregatorOrDefault(), GetDataServiceOrDefault(), GetViewModelServiceOrDefault()),
                                                            new SingletonVariableViewerViewModel(new SingletonVariableGraphicModel(theAggregateVariable)));
        }

        private VariableDomainExpressionModel GetExpressionOrDefault()
        {
            return this.domain ?? new VariableDomainExpressionModel();
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
