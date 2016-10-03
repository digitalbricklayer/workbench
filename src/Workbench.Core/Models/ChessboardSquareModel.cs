using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An individual chessboard square.
    /// </summary>
    [Serializable]
    public class ChessboardSquareModel : AbstractModel
    {
        private readonly ChessPieceModel piece;
        private readonly Point location;

        public ChessboardSquareModel(ChessPieceModel thePiece)
        {
            this.location = thePiece.Position;
            this.piece = thePiece;
        }

        public ChessboardSquareModel(Point theLocation)
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

        /// <summary>
        /// Create an empty chessboard square.
        /// </summary>
        /// <param name="theLocation">Location on the board.</param>
        /// <returns>Empty boad square.</returns>
        public static ChessboardSquareModel CreateEmpty(Point theLocation)
        {
            return new ChessboardSquareModel(theLocation);
        }

        /// <summary>
        /// Create a chessboard square with a piece occupying it.
        /// </summary>
        /// <param name="thePiece">Peice.</param>
        /// <returns>Board square with a piece occupying it.</returns>
        public static ChessboardSquareModel CreateWith(ChessPieceModel thePiece)
        {
            Contract.Requires<ArgumentNullException>(thePiece != null);
            return new ChessboardSquareModel(thePiece);
        }
    }
}
