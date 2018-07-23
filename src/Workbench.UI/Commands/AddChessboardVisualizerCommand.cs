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

        public AddChessboardVisualizerCommand(WorkAreaViewModel theWorkArea,
                                              TitleBarViewModel theTitleBar,
                                              IEventAggregator theEventAggregator,
                                              IDataService theDataService)
        {
            Contract.Requires<ArgumentNullException>(theWorkArea != null);
            Contract.Requires<ArgumentNullException>(theTitleBar != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);

            this.workArea = theWorkArea;
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
#if false
            this.workArea.ChangeSelectedDisplayTo("Editor");
#endif
            var newChessboardModel = new ChessboardModel(new ModelName("Chessboard"));
            var newVisualizerModel = new ChessboardVisualizerModel(newChessboardModel, new VisualizerTitle());
            this.workArea.AddChessboardVisualizer(newVisualizerModel);
            this.titleBar.UpdateTitle();
        }
    }
}
