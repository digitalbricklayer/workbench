using System;
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
        private VisualizerBindingExpressionModel binding;

        /// <summary>
        /// Initialize the chessboard visualizer with a screen location.
        /// </summary>
        /// <param name="theName">Name of the chessboard.</param>
        /// <param name="theLocation">Screen location.</param>
        /// <param name="rawBindingExpression">Raw binding expression.</param>
        public ChessboardVisualizerModel(string theName, Point theLocation, string rawBindingExpression)
            : base(theName, theLocation)
        {
            this.chessboard = new ChessboardModel();
            Binding = new VisualizerBindingExpressionModel(this, rawBindingExpression);
        }

        /// <summary>
        /// Initialize the chessboard visualizer with a screen location.
        /// </summary>
        /// <param name="theName">Name of the chessboard.</param>
        /// <param name="theLocation">Screen location.</param>
        public ChessboardVisualizerModel(string theName, Point theLocation)
            : base(theName, theLocation)
        {
            this.chessboard = new ChessboardModel();
            Binding = new VisualizerBindingExpressionModel(this);
        }

        /// <summary>
        /// Gets the visualizer binding expression.
        /// </summary>
        public VisualizerBindingExpressionModel Binding
        {
            get { return this.binding; }
            private set
            {
                this.binding = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Update the visualizer from the solution.
        /// </summary>
        /// <param name="theContext">Context to update the visualizer.</param>
        public override void UpdateFrom(VisualizerUpdateContext theContext)
        {
            Binding.ExecuteWith(theContext);
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
