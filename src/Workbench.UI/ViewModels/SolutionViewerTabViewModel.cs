using System;
using System.Diagnostics.Contracts;
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

        public SolutionViewerTabViewModel()
        {
            DisplayName = "Solution";
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
        /// Add a label.
        /// </summary>
        /// <param name="newLabel">New label.</param>
        public void AddLabel(LabelModel newSingletonLabelViewModel)
        {
            Contract.Requires<ArgumentNullException>(newSingletonLabelViewModel != null);
            Viewer.AddLabel(newSingletonLabelViewModel);
        }

        public void BindTo(SolutionModel theSolution)
        {
            Viewer.Reset();
            Viewer.BindTo(theSolution);
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