using System.Collections.Generic;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Visualizer for a chessboard.
    /// </summary>
    public class ChessboardVisualizerModel : VisualizerModel
    {
        private readonly ChessboardModel chessboard;

        /// <summary>
        /// Initialize the chessboard visualizer with a screen location.
        /// </summary>
        /// <param name="theLocation">Screen location.</param>
        public ChessboardVisualizerModel(Point theLocation)
            : base("Chessboard", theLocation)
        {
            this.chessboard = new ChessboardModel();
        }

        /// <summary>
        /// Hydrate the visualizer from a value.
        /// </summary>
        /// <param name="theValue">Value bound to a variable.</param>
        public override void Hydrate(ValueModel theValue)
        {
            base.Hydrate(theValue);
            for (var x = 1; x <= theValue.Values.Count; x++)
            {
                for (var y = 1; y <= theValue.Values.Count; y++)
                {
                    if (theValue.GetValueAt(x) == y)
                    {
                        // A square with a queen
                        var squareLocation = new Point(x, y);
                        var theQueen = ChessPieceModel.CreateFrom(squareLocation,
                                                                  Player.White,
                                                                  PieceType.Queen);
                        var newSquare = ChessboardSquareModel.CreateWith(squareLocation, theQueen);
                        this.chessboard.Add(newSquare);
                        // Only one queen can be present on each row...
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Get all squares occupied by the type of piece.
        /// </summary>
        /// <param name="theTypeOfPiece">Type of piece to search for.</param>
        /// <returns>Collection of matching squares.</returns>
        public IReadOnlyCollection<ChessboardSquareModel> GetSquaresOccupiedBy(PieceType theTypeOfPiece)
        {
            return this.chessboard.GetSquaresOccupiedBy(theTypeOfPiece);
        }
    }
}
