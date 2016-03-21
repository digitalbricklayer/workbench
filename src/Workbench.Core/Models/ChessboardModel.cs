using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A chessboard model.
    /// </summary>
    public class ChessboardModel : AbstractModel
    {
        private ObservableCollection<ChessboardSquareModel> pieces;

        /// <summary>
        /// Initialize a chessboard with default values.
        /// </summary>
        public ChessboardModel()
        {
            this.pieces = new ObservableCollection<ChessboardSquareModel>();
        }

        /// <summary>
        /// Gets or sets the chess pieces.
        /// </summary>
        public ObservableCollection<ChessboardSquareModel> Pieces
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
        /// Add a piece the chessboard.
        /// </summary>
        /// <param name="newPiece">New piece.</param>
        public void Add(ChessboardSquareModel newPiece)
        {
            Contract.Requires<ArgumentNullException>(newPiece != null);
            this.Pieces.Add(newPiece);
        }
    }
}
