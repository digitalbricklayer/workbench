using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A chessboard model.
    /// </summary>
    [Serializable]
    public class ChessboardModel : Model
    {
        private const int DefaultSize = 8;
        private ObservableCollection<ChessboardSquareModel> _squares;
        private int _size = DefaultSize;

        /// <summary>
        /// Initialize a chessboard with a name.
        /// </summary>
        /// <param name="modelName"></param>
        public ChessboardModel(ModelName modelName)
            : base(modelName)
        {
            Squares = new ObservableCollection<ChessboardSquareModel>();
            InitializeBoard();
        }

        /// <summary>
        /// Initialize a chessboard with default values.
        /// </summary>
        public ChessboardModel()
        {
            Squares = new ObservableCollection<ChessboardSquareModel>();
            InitializeBoard();
        }

        /// <summary>
        /// Gets or sets the size of the chessboard.
        /// </summary>
        public int Size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the chess pieces.
        /// </summary>
        public ObservableCollection<ChessboardSquareModel> Squares
        {
            get => _squares;
            set
            {
                _squares = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Add a piece the chessboard.
        /// </summary>
        /// <param name="newSquare">New chess piece.</param>
        public void Add(ChessboardSquareModel newSquare)
        {
            if (newSquare.Piece.Type == PieceType.Empty)
                throw new ArgumentException(nameof(newSquare));

            // Convert a x, y coordinate into a one dimensional array index
            var index = Convert.ToInt32((newSquare.Pos.X - 1) * Size + (newSquare.Pos.Y - 1));
            Squares[index] = newSquare;
        }

        /// <summary>
        /// Get all squares occupied by the type of piece.
        /// </summary>
        /// <param name="theTypeOfPiece">Type of piece to search for.</param>
        /// <returns>Collection of matching squares.</returns>
        public IReadOnlyCollection<ChessboardSquareModel> GetSquaresOccupiedBy(PieceType theTypeOfPiece)
        {
            return Squares.Where(square => square.Piece.Type == theTypeOfPiece)
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
                    var newSquare = ChessboardSquareModel.CreateEmpty(squareLocation);
                    Squares.Add(newSquare);
                }
            }
        }
    }
}
