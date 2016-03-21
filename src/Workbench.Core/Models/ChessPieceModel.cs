using System;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An individual piece on a chessboard.
    /// </summary>
    [Serializable]
    public class ChessPieceModel : AbstractModel
    {
        private Point position;
        private PieceType type;
        private Player player;

        public ChessPieceModel(Point theLocation,
                               Player thePlayer,
                               PieceType thePiece)
        {
            this.Position = theLocation;
            this.Player = thePlayer;
            this.Type = thePiece;
        }

        /// <summary>
        /// Gets or sets the piece position on the board.
        /// </summary>
        public Point Position
        {
            get { return this.position; }
            set
            {
                this.position = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the piece type.
        /// </summary>
        public PieceType Type
        {
            get { return this.type; }
            set
            {
                this.type = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the player color.
        /// </summary>
        public Player Player
        {
            get { return this.player; }
            set
            {
                this.player = value;
                this.OnPropertyChanged();
            }
        }

        public static ChessPieceModel CreateFrom(Point theLocation, Player thePlayer, PieceType thePiece)
        {
            return new ChessPieceModel(theLocation, thePlayer, thePiece);
        }
    }
}
