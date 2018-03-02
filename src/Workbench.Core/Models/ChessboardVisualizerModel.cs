using System;
using System.Collections.Generic;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Visualizer for a chessboard.
    /// </summary>
    [Serializable]
    public class ChessboardVisualizerModel : VisualizerModel
    {
        private readonly ChessboardModel chessboard;

        /// <summary>
        /// Initialize the chessboard visualizer with a screen location.
        /// </summary>
        /// <param name="theName">Text of the chessboard.</param>
        /// <param name="theLocation">Screen location.</param>
        public ChessboardVisualizerModel(ChessboardModel theBoard, VisualizerTitle theTitle, Point theLocation)
            : base(theBoard, theTitle, theLocation)
        {
            this.chessboard = theBoard;
        }

        /// <summary>
        /// Gets the chessboard model.
        /// </summary>
        public ChessboardModel Model
        {
            get
            {
                return this.chessboard;
            }
        }

        /// <summary>
        /// Update the chessboard visualizer with call arguments.
        /// </summary>
        /// <param name="theCall">Call arguments.</param>
        public override void UpdateWith(VisualizerCall theCall)
        {
            var x = theCall.GetArgumentByName("x");
            var y = theCall.GetArgumentByName("y");
            var playerDescription = theCall.GetArgumentByName("side");
            var pieceDescription = theCall.GetArgumentByName("piece");

            this.chessboard.Add(new ChessboardSquareModel(new ChessPieceModel(new Point(ConvertLocationFrom(x), ConvertLocationFrom(y)),
                                                          ConvertPlayerFrom(playerDescription),
                                                          ConvertToPieceTypeFrom(pieceDescription))));
        }

        private int ConvertLocationFrom(string value)
        {
            int n;
            bool isNumeric = int.TryParse(value, out n);

            if (isNumeric) return n;
            return default(int);
        }

        private Player ConvertPlayerFrom(string playerDescription)
        {
            var lowerCasePlayerDescription = playerDescription.ToLower();
            switch (lowerCasePlayerDescription)
            {
                case "white":
                    return Player.White;

                case "black":
                    return Player.Black;

                default:
                    throw new NotImplementedException();
            }
        }

        private PieceType ConvertToPieceTypeFrom(string pieceDescription)
        {
            var lowerCasePieceDescription = pieceDescription.ToLower();
            switch (lowerCasePieceDescription)
            {
                case "queen":
                    return PieceType.Queen;

                default:
                    throw new NotImplementedException();
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
