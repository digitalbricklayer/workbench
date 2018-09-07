using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the built-in solution viewer.
    /// </summary>
    public sealed class SolutionViewerTabViewModel : Conductor<IScreen>.Collection.AllActive, IWorkspaceTabViewModel
    {
        private SolutionViewerPanelViewModel _viewer;
        private SolutionStatsPanelViewModel _stats;
        private string _text;

        /// <summary>
        /// Initialize a default solution view tab with default values.
        /// </summary>
        public SolutionViewerTabViewModel()
        {
            TabText = DisplayName = "Solution";
        }

        /// <summary>
        /// Gets or sets the solution viewer panel view model.
        /// </summary>
        public SolutionViewerPanelViewModel Viewer
        {
            get => _viewer;
            set
            {
                _viewer = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the solution stats view model.
        /// </summary>
        public SolutionStatsPanelViewModel Stats
        {
            get => _stats;
            set
            {
                _stats = value;
                NotifyOfPropertyChange();
            }
        }

        public bool CloseTabIsVisible => false;

        /// <summary>
        /// Gets or sets the table text.
        /// </summary>
        public string TabText
        {
            get => _text;
            set
            {
                _text = value; 
                NotifyOfPropertyChange();
            }
        }

        public void BindTo(SolutionModel theSolution)
        {
            Viewer = new SolutionViewerPanelViewModel();
            Viewer.BindTo(theSolution);
            Stats = new SolutionStatsPanelViewModel();
            Stats.BindTo(theSolution);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Viewer = new SolutionViewerPanelViewModel();
            Items.Add(Viewer);
            Stats = new SolutionStatsPanelViewModel();
            Items.Add(Stats);
        }
    }
}