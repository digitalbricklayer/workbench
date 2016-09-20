using System;
using System.Collections.Generic;
using System.Windows;
using Workbench.Core.Solver;

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

#if true

        /// <summary>
        /// Update the visualizer from the solution.
        /// </summary>
        /// <param name="theSnapshot">Solution snapshot.</param>
        public override void UpdateFrom(SolutionSnapshot theSnapshot)
        {
            Binding.ExecuteWith(theSnapshot);
        }

        /// <summary>
        /// Update the chessboard visualizer with call arguments.
        /// </summary>
        /// <param name="theCall">Call arguments.</param>
        public override void UpdateWith(VisualizerCall theCall)
        {
            throw new NotImplementedException();
        }

#if false
    /// <summary>
    /// ExecuteWith the visualizer from a value.
    /// </summary>
    /// <param name="theSnapshot">Solution snapshot.</param>
        public override void UpdateFrom(SolutionSnapshot theSnapshot)
        {
            for (var x = 1; x <= theValue.Values.Count; x++)
            {
                for (var y = 1; y <= theValue.Values.Count; y++)
                {
                    if (theValue.GetValueAt(x-1) == y)
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
#endif
#endif

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
