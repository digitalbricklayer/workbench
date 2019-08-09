using System;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the chessboard.
    /// </summary>
    public class ChessboardViewModel : Screen
    {
        private ChessboardModel _model;
        private ObservableCollection<ChessboardSquareViewModel> _squares;

        /// <summary>
        /// Initialize a chessboard view model with a chessboard model.
        /// </summary>
        /// <param name="theModel">Chessboard model.</param>
        public ChessboardViewModel(ChessboardModel theModel)
        {
            Model = theModel;
            Squares = new ObservableCollection<ChessboardSquareViewModel>();
        }

        /// <summary>
        /// Gets or sets the chessboard model.
        /// </summary>
        public ChessboardModel Model
        {
            get => _model;
            set
            {
                _model = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the chess pieces.
        /// </summary>
        public ObservableCollection<ChessboardSquareViewModel> Squares
        {
            get => _squares;
            set
            {
                _squares = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Update the view model from the chessboard model.
        /// </summary>
        public void UpdateFromModel()
        {
            SynchronizeWithModel();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            SynchronizeWithModel();
        }

        private void SynchronizeWithModel()
        {
            Squares.Clear();
            foreach (var aSquare in Model.Squares)
            {
                Squares.Add(new ChessboardSquareViewModel(aSquare));
            }
        }
    }
}
