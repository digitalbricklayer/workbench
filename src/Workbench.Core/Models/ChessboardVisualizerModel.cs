using System.Windows;

namespace Workbench.Core.Models
{
    public class ChessboardVisualizerModel : VisualizerModel
    {
        public ChessboardVisualizerModel(Point theLocation)
            : base("Chessboard", theLocation)
        {
        }
    }
}
