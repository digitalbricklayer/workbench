using System;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench.Commands
{
    /// <summary>
    /// Add a new visualizer to the solution designer.
    /// </summary>
    public class AddVisualizerCommand : CommandBase
    {
        private readonly WorkspaceViewModel workspace;
        private readonly TitleBarViewModel titleBar;
        private readonly IDataService dataService;
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Initialize an add visualizer command with a workspace and titlebar.
        /// </summary>
        /// <param name="theWorkspace">Workspace view model.</param>
        /// <param name="theTitleBar">Title bar view model.</param>
        /// <param name="theEventAggregator">Event aggregator.</param>
        /// <param name="theDataService">Event aggregator.</param>
        public AddVisualizerCommand(WorkspaceViewModel theWorkspace,
                                    TitleBarViewModel theTitleBar,
                                    IEventAggregator theEventAggregator,
                                    IDataService theDataService)
        {
            if (theWorkspace == null)
                throw new ArgumentNullException("theWorkspace");

            if (theTitleBar == null)
                throw new ArgumentNullException("theTitleBar");

            if (theEventAggregator == null)
                throw new ArgumentNullException("theEventAggregator");

            if (theDataService == null)
                throw new ArgumentNullException("theDataService");

            this.workspace = theWorkspace;
            this.titleBar = theTitleBar;
            this.eventAggregator = theEventAggregator;
            this.dataService = theDataService;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. 
        /// If the command does not require data to be passed, this object can be set to null.
        /// </param>
        public override void Execute(object parameter)
        {
            var newVisualizerLocation = Mouse.GetPosition(Application.Current.MainWindow);
            var newVisualizerModel = new VariableVisualizerModel(newVisualizerLocation);
            this.CreateDesigner(newVisualizerModel);
            this.CreateViewer(newVisualizerModel);
            this.titleBar.UpdateTitle();
        }

        private void CreateViewer(VariableVisualizerModel newVisualizerModel)
        {
            var visualizerViewerViewModel = new VariableVisualizerViewerViewModel(newVisualizerModel);
            this.workspace.AddViewer(visualizerViewerViewModel);
        }

        private void CreateDesigner(VariableVisualizerModel newVisualizerModel)
        {
            var visualizerDesignViewModel = new VariableVisualizerDesignViewModel(newVisualizerModel,
                                                                                  this.eventAggregator,
                                                                                  this.dataService);
            this.workspace.AddDesigner(visualizerDesignViewModel);
        }
    }
}
