using System.Windows;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for a chessboard square.
    /// </summary>
    public class ChessboardSquareViewModel : Screen
    {
        private ChessboardSquareModel _model;

        /// <summary>
        /// Initialize a chessboard square view model with a square model.
        /// </summary>
        /// <param name="theModel"></param>
        public ChessboardSquareViewModel(ChessboardSquareModel theModel)
        {
            Model = theModel;
        }

        /// <summary>
        /// Gets or sets the chessboard square model.
        /// </summary>
        public ChessboardSquareModel Model
        {
            get => _model;
            set
            {
                _model = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the piece.
        /// </summary>
        public ChessPieceModel Piece => Model.Piece;

        /// <summary>
        /// Gets the player.
        /// </summary>
        public Player Player => Model.Player;

        /// <summary>
        /// Gets the position using a one based index.
        /// </summary>
        public Point Pos => Model.Pos;

        /// <summary>
        /// Gets the position in using a zero based index.
        /// </summary>
        public Point Pos2 => Model.Pos2;

        /// <summary>
        /// Gets the piece type.
        /// </summary>
        public PieceType Type => Model.Type;

        /// <summary>
        /// Gets whether the board square has a piece occupying it.
        /// </summary>
        public bool HasPiece => Model.HasPiece;
    }
}