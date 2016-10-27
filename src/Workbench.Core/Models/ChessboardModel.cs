using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A chessboard model.
    /// </summary>
    public class ChessboardModel : AbstractModel
    {
        private const int DefaultSize = 8;
        private ObservableCollection<ChessboardSquareModel> pieces;
        private int size = DefaultSize;

        /// <summary>
        /// Initialize a chessboard with default values.
        /// </summary>
        public ChessboardModel()
        {
            this.pieces = new ObservableCollection<ChessboardSquareModel>();
            InitializeBoard();
        }

        /// <summary>
        /// Gets or sets the size of the chessboard.
        /// </summary>
        public int Size
        {
            get { return this.size; }
            set
            {
                this.size = value;
                OnPropertyChanged();
            }
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
        /// <param name="newSquare">New chess piece.</param>
        public void Add(ChessboardSquareModel newSquare)
        {
            Contract.Requires<ArgumentNullException>(newSquare != null);
            Contract.Requires<ArgumentException>(newSquare.Piece.Type != PieceType.Empty);
            // Convert a x, y coordinate into a one dimensional array index
            var index = Convert.ToInt32(((newSquare.Pos.X - 1) * Size) + (newSquare.Pos.Y - 1));
            Pieces[index] = newSquare;
        }

        /// <summary>
        /// Get all squares occupied by the type of piece.
        /// </summary>
        /// <param name="theTypeOfPiece">Type of piece to search for.</param>
        /// <returns>Collection of matching squares.</returns>
        public IReadOnlyCollection<ChessboardSquareModel> GetSquaresOccupiedBy(PieceType theTypeOfPiece)
        {
            return this.Pieces.Where(square => square.Piece.Type == theTypeOfPiece)
                              .ToList();
        }

        private void InitializeBoard()
        {
            for (var col = 1; col <= Size; col++)
            {
                for (var row = 1; row <= Size; row++)
                {
                    // An empty space on the board.
                    var squareLocation = new Point(col, row);
                    var newPiece = ChessboardSquareModel.CreateEmpty(squareLocation);
                    Pieces.Add(newPiece);
                }
            }
        }
    }
}
