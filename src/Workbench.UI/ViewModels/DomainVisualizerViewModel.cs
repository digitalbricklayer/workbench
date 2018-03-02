using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class DomainVisualizerViewModel : VisualizerViewModel
    {
        public DomainVisualizerViewModel(DomainModel theDomain, DomainEditorViewModel theEditor, DomainViewerViewModel theViewer)
            : base(theDomain, theEditor, theViewer)
        {
            DomainEditor = theEditor;
            DomainViewer = theViewer;
        }

        public DomainViewerViewModel DomainViewer { get; set; }

        public DomainEditorViewModel DomainEditor { get; set; }
    }
}
