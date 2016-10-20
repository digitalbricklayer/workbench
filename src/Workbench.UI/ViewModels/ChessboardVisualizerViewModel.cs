using System;
using System.Diagnostics.Contracts;

namespace Workbench.ViewModels
{
    public sealed class ChessboardVisualizerViewModel : VisualizerViewModel
    {
        public ChessboardVisualizerViewModel(ChessboardVisualizerDesignViewModel theDesigner, ChessboardVisualizerViewerViewModel theViewer)
        {
            Contract.Requires<ArgumentNullException>(theDesigner != null);
            Contract.Requires<ArgumentNullException>(theViewer != null);
            Designer = theDesigner;
            Viewer = theViewer;
        }
    }
}
