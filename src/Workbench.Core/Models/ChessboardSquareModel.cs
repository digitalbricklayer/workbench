using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A chessboard square.
    /// </summary>
    [Serializable]
    public class ChessboardSquareModel : AbstractModel
    {
        private readonly ChessPieceModel piece;
        private readonly Point location;

        /// <summary>
        /// Initialize a square occupied by the piece.
        /// </summary>
        /// <param name="thePiece">Piece to occupy the square.</param>
        public ChessboardSquareModel(ChessPieceModel thePiece)
        {
            this.location = thePiece.Position;
            this.piece = thePiece;
        }

        /// <summary>
        /// Initialize an empty square.
        /// </summary>
        /// <param name="theLocation"></param>
        public ChessboardSquareModel(Point theLocation)
        {
            this.location = theLocation;
            this.piece = new ChessPieceModel(theLocation, Player.White, PieceType.Empty);
        }

        /// <summary>
        /// Gets the piece.
        /// </summary>
        public ChessPieceModel Piece
        {
            get { return piece; }
        }

        /// <summary>
        /// Gets the piece player.
        /// </summary>
        public Player Player
        {
            get
            {
                return this.piece.Player;
            }
        }

        /// <summary>
        /// Gets the position using a one based index.
        /// </summary>
        public Point Pos
        {
            get { return location; }
        }

        /// <summary>
        /// Gets the position using a zero based index.
        /// </summary>
        public Point Pos2
        {
            get { return new Point(location.X - 1, location.Y - 1); }
        }

        /// <summary>
        /// Gets the piece type.
        /// </summary>
        public PieceType Type
        {
            get
            {
                return this.piece.Type;
            }
        }

        /// <summary>
        /// Gets whether the board square has a piece in it.
        /// </summary>
        public bool HasPiece
        {
            get
            {
                return Piece.Type != PieceType.Empty;
            }
        }

        /// <summary>
        /// Create an empty chessboard square.
        /// </summary>
        /// <param name="theLocation">Location on the board.</param>
        /// <returns>Empty board square.</returns>
        public static ChessboardSquareModel CreateEmpty(Point theLocation)
        {
            return new ChessboardSquareModel(theLocation);
        }

        /// <summary>
        /// Create a chessboard square with a piece occupying it.
        /// </summary>
        /// <param name="thePiece">Piece.</param>
        /// <returns>Board square with a piece occupying it.</returns>
        public static ChessboardSquareModel CreateWith(ChessPieceModel thePiece)
        {
            Contract.Requires<ArgumentNullException>(thePiece != null);
            return new ChessboardSquareModel(thePiece);
        }
    }
}
