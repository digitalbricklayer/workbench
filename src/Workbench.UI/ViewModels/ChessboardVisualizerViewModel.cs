using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public sealed class ChessboardVisualizerViewModel : VisualizerViewModel
    {
        public ChessboardVisualizerViewModel(ChessboardModel thechessboard, ChessboardEditorViewModel theEditor, ChessboardViewerViewModel theViewer)
            : base(thechessboard, theEditor, theViewer)
        {
        }
    }
}
