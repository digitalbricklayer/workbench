using System;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Model for a chess square on a chessboard.
    /// </summary>
    [Serializable]
    public class ChessSquareModel : AbstractModel
    {
        private readonly ChessPieceModel piece;
        private readonly Point location;

        public ChessSquareModel(Point theLocation, ChessPieceModel thePiece)
        {
            this.location = theLocation;
            this.piece = thePiece;
        }

        public ChessSquareModel(Point theLocation)
        {
            this.location = theLocation;
        }

        public ChessPieceModel Piece
        {
            get { return piece; }
        }

        public Point Location
        {
            get { return location; }
        }

        /// <summary>
        /// Gets whether the board square has a piece in it.
        /// </summary>
        public bool HasPiece
        {
            get
            {
                return this.Piece != null;
            }
        }

        public static ChessSquareModel CreateEmpty(Point theLocation)
        {
            return new ChessSquareModel(theLocation);
        }

        public static ChessSquareModel CreateWith(Point theLocation,
                                                  ChessPieceModel thePiece)
        {
            return new ChessSquareModel(theLocation, thePiece);
        }
    }
}
