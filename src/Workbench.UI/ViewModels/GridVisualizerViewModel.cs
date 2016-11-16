using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class GridVisualizerViewModel : VisualizerViewModel
    {
        public GridVisualizerViewModel(GridVisualizerDesignerViewModel theDesigner,
                                       GridVisualizerViewerViewModel theViewer)
        {
            Contract.Requires<ArgumentNullException>(theDesigner != null);
            Contract.Requires<ArgumentNullException>(theViewer != null);
            Designer = theDesigner;
            Viewer = theViewer;
            Model = theViewer.MapModel;
        }

        public GridVisualizerModel Model { get; private set; }
    }
}
