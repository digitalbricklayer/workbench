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
        /// Hydrate the visualizer from the value.
        /// </summary>
        /// <param name="theValue">Value bound to a variable.</param>
        public override void Hydrate(ValueModel theValue)
        {
            base.Hydrate(theValue);
            var counter = 0;
            foreach (var aValue in theValue.Values)
            {
                counter++;
                if (aValue == 1)
                {
                    // A square with a queen
                    var squareLocation = BoardConvert.ToPoint(counter);
                    var theQueen = ChessPieceModel.CreateFrom(squareLocation,
                                                              Player.White,
                                                              PieceType.Queen);
                    var newPiece = ChessboardSquareModel.CreateWith(squareLocation, theQueen);
                    this.chessboard.Add(newPiece);
                }
                else
                {
                    // An empty space on the board.
                    var squareLocation = BoardConvert.ToPoint(counter);
                    var newPiece = ChessboardSquareModel.CreateEmpty(squareLocation);
                    this.chessboard.Add(newPiece);
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
