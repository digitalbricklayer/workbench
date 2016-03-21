using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A chessboard model.
    /// </summary>
    public class ChessboardModel : AbstractModel
    {
        private ObservableCollection<ChessboardSquareModel> pieces;

        /// <summary>
        /// Initialize a chessboard with default values.
        /// </summary>
        public ChessboardModel()
        {
            this.pieces = new ObservableCollection<ChessboardSquareModel>();
        }

        /// <summary>
        /// Gets or sets the chess pieces.
        /// </summary>
        public ObservableCollection<ChessboardSquareModel> Pieces
        {
            get { return this.pieces; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.pieces = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Add a piece the chessboard.
        /// </summary>
        /// <param name="newPiece">New piece.</param>
        public void Add(ChessboardSquareModel newPiece)
        {
            Contract.Requires<ArgumentNullException>(newPiece != null);
            this.Pieces.Add(newPiece);
        }

        /// <summary>
        /// Get all squares occupied by the type of piece.
        /// </summary>
        /// <param name="theTypeOfPiece">Type of piece to search for.</param>
        /// <returns>Collection of matching squares.</returns>
        public IReadOnlyCollection<ChessboardSquareModel> GetSquaresOccupiedBy(PieceType theTypeOfPiece)
        {
            return this.Pieces.Where(square => square.HasPiece && square.Piece.Type == theTypeOfPiece)
                              .ToList();
        }
    }
}
