using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Visualizer for a chessboard.
    /// </summary>
    public class ChessboardVisualizerModel : VisualizerModel
    {
        private ObservableCollection<ChessSquareModel> pieces;

        /// <summary>
        /// Initialize the chessboard visualizer with a screen location.
        /// </summary>
        /// <param name="theLocation">Screen location.</param>
        public ChessboardVisualizerModel(Point theLocation)
            : base("Chessboard", theLocation)
        {
            this.pieces = new ObservableCollection<ChessSquareModel>();
        }

        /// <summary>
        /// Gets or sets the chess pieces.
        /// </summary>
        public ObservableCollection<ChessSquareModel> Pieces
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
                    var newPiece = ChessSquareModel.CreateWith(squareLocation, theQueen);
                    this.Pieces.Add(newPiece);
                }
                else
                {
                    // An empty space on the board.
                    var squareLocation = BoardConvert.ToPoint(counter);
                    var newPiece = ChessSquareModel.CreateEmpty(squareLocation);
                    this.Pieces.Add(newPiece);
                }
            }
        }
    }
}
