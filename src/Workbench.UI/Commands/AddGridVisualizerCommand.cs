using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench.Commands
{
    /// <summary>
    /// Add a new chessboard visualizer to the solution designer.
    /// </summary>
    public class AddGridVisualizerCommand : CommandBase
    {
        private readonly WorkspaceViewModel workspace;
        private readonly TitleBarViewModel titleBar;
        private readonly IDataService dataService;
        private readonly IEventAggregator eventAggregator;
        private readonly IViewModelService viewModelService;

        public AddGridVisualizerCommand(WorkspaceViewModel theWorkspace,
                                       TitleBarViewModel theTitleBar,
                                       IEventAggregator theEventAggregator,
                                       IDataService theDataService,
                                       IViewModelService theViewModelService)
        {
            Contract.Requires<ArgumentNullException>(theWorkspace != null);
            Contract.Requires<ArgumentNullException>(theTitleBar != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theViewModelService != null);

            this.workspace = theWorkspace;
            this.titleBar = theTitleBar;
            this.eventAggregator = theEventAggregator;
            this.dataService = theDataService;
            this.viewModelService = theViewModelService;
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
            this.workspace.ChangeSelectedDisplayTo("Designer");
            var newVisualizerModel = new GridVisualizerModel("Map", newVisualizerLocation, new[] { "X", "Y" }, new[] { new GridRowModel(), new GridRowModel() });
            this.workspace.AddMapVisualizer(CreateMapVisualizer(newVisualizerModel));
            this.titleBar.UpdateTitle();
        }

        private GridVisualizerViewModel CreateMapVisualizer(GridVisualizerModel newVisualizerModel)
        {
            return new GridVisualizerViewModel(CreateDesigner(newVisualizerModel),
                                              new GridVisualizerViewerViewModel(newVisualizerModel));
        }

        private GridVisualizerDesignerViewModel CreateDesigner(GridVisualizerModel newVisualizerModel)
        {
            return new GridVisualizerDesignerViewModel(newVisualizerModel,
                                                      this.eventAggregator,
                                                      this.dataService,
                                                      this.viewModelService);
        }
    }
}
