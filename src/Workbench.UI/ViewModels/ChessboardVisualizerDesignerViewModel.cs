using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class ChessboardVisualizerDesignerViewModel : EditorViewModel
    {
        private ChessboardViewModel board;

        public ChessboardVisualizerDesignerViewModel(ChessboardVisualizerModel theChessboardVisualizerModel,
                                                     IEventAggregator theEventAggregator,
                                                     IDataService theDataService,
                                                     IViewModelService theViewModelService)
            : base(theChessboardVisualizerModel, theEventAggregator, theDataService, theViewModelService)
        {
            Contract.Requires<ArgumentNullException>(theChessboardVisualizerModel != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theViewModelService != null);

            Model = theChessboardVisualizerModel;
            Board = new ChessboardViewModel(theChessboardVisualizerModel.Model);
        }

        public ChessboardViewModel Board
        {
            get { return this.board; }
            set
            {
                this.board = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
