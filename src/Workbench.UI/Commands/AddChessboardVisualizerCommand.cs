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
    public class AddChessboardVisualizerCommand : CommandBase
    {
        private readonly WorkAreaViewModel workArea;
        private readonly TitleBarViewModel titleBar;
        private readonly IDataService dataService;
        private readonly IEventAggregator eventAggregator;
        private readonly IViewModelService viewModelService;

        public AddChessboardVisualizerCommand(WorkAreaViewModel theWorkArea,
                                              TitleBarViewModel theTitleBar,
                                              IEventAggregator theEventAggregator,
                                              IDataService theDataService,
                                              IViewModelService theViewModelService)
        {
            Contract.Requires<ArgumentNullException>(theWorkArea != null);
            Contract.Requires<ArgumentNullException>(theTitleBar != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theViewModelService != null);

            this.workArea = theWorkArea;
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
            this.workArea.ChangeSelectedDisplayTo("Editor");
            var newChessboardModel = new ChessboardModel(new ModelName("Chessboard"));
            var newVisualizerModel = new ChessboardVisualizerModel(newChessboardModel, new VisualizerTitle(), newVisualizerLocation);
            this.workArea.AddChessboardVisualizer(CreateChessboardVisualizer(newVisualizerModel));
            this.titleBar.UpdateTitle();
        }

        private ChessboardVisualizerViewModel CreateChessboardVisualizer(ChessboardVisualizerModel newVisualizerModel)
        {
            return new ChessboardVisualizerViewModel(CreateDesigner(newVisualizerModel),
                                                     new ChessboardVisualizerViewerViewModel(newVisualizerModel));
        }

        private ChessboardVisualizerDesignerViewModel CreateDesigner(ChessboardVisualizerModel newVisualizerModel)
        {
            return new ChessboardVisualizerDesignerViewModel(newVisualizerModel,
                                                           this.eventAggregator,
                                                           this.dataService,
                                                           this.viewModelService);
        }
    }
}
