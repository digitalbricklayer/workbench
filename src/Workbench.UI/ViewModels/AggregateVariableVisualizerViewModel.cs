using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class AggregateVariableVisualizerViewModel  : VariableVisualizerViewModel
    {
        public AggregateVariableVisualizerViewModel(AggregateVariableModel theVariable, AggregateVariableEditorViewModel theEditor, AggregateVariableViewerViewModel theViewer)
            : base(theVariable, theEditor, theViewer)
        {
            AggregateEditor = theEditor;
            AggregateViewer = theViewer;
        }

        public AggregateVariableViewerViewModel AggregateViewer { get; set; }

        public AggregateVariableEditorViewModel AggregateEditor { get; set; }
    }

    public class AggregateVariableEditorViewModel : VariableEditorViewModel
    {
        public AggregateVariableEditorViewModel(AggregateVariableGraphicModel theGraphicModel, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theGraphicModel, theEventAggregator, theDataService, theViewModelService)
        {
            AggregateVariableGraphic = theGraphicModel;
        }

        public AggregateVariableGraphicModel AggregateVariableGraphic { get; set; }
        public string NumberVariables { get; set; }
    }

    public class AggregateVariableViewerViewModel : VariableViewerViewModel
    {
        public AggregateVariableViewerViewModel(AggregateVariableGraphicModel theGraphicModel)
            : base(theGraphicModel)
        {
            AggregateVariableGraphic = theGraphicModel;
        }

        public AggregateVariableGraphicModel AggregateVariableGraphic { get; set; }
    }

    public class VariableVisualizerViewModel : VisualizerViewModel
    {
        public VariableVisualizerViewModel(VariableModel theVariable, VariableEditorViewModel theEditor, VariableViewerViewModel theViewer)
            : base(theVariable, theEditor, theViewer)
        {
            Variable = theVariable;
            VariableEditor = theEditor;
            VariableViewer = theViewer;
        }

        public VariableViewerViewModel VariableViewer { get; set; }

        public VariableEditorViewModel VariableEditor { get; set; }

        public VariableModel Variable { get; }
    }

    public class VariableViewerViewModel : ViewerViewModel
    {
        public VariableViewerViewModel(GraphicModel theGraphicModel)
            : base(theGraphicModel)
        {
        }
    }

    public class VariableEditorViewModel : EditorViewModel
    {
        private VariableDomainExpressionViewModel domainExpression;

        public VariableEditorViewModel(VariableGraphicModel theGraphicModel, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theGraphicModel, theEventAggregator, theDataService, theViewModelService)
        {
            VariableGraphic = theGraphicModel;
            this.domainExpression = new VariableDomainExpressionViewModel(theGraphicModel.DomainExpression);
        }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public VariableDomainExpressionViewModel DomainExpression
        {
            get
            {
                return this.domainExpression;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.domainExpression = value;
                if (this.Model != null)
                    VariableGraphic.DomainExpression = this.domainExpression.Model;
                NotifyOfPropertyChange();
            }
        }

        public VariableGraphicModel VariableGraphic { get; set; }
        public bool IsAggregate { get; set; }
    }
}
