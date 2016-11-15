using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class MapVisualizerViewModel : VisualizerViewModel
    {
        public MapVisualizerViewModel(MapVisualizerDesignerViewModel theDesigner,
                                      MapVisualizerViewerViewModel theViewer)
        {
            Contract.Requires<ArgumentNullException>(theDesigner != null);
            Contract.Requires<ArgumentNullException>(theViewer != null);
            Designer = theDesigner;
            Viewer = theViewer;
            Model = theViewer.MapModel;
        }

        public MapVisualizerModel Model { get; private set; }
    }
}
