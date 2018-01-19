using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution viewer.
    /// </summary>
    public sealed class WorkspaceViewerViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private SnapshotViewerViewModel snapshot;
        private WorkspaceViewerPanelViewModel viewer;

        /// <summary>
        /// Initialize the solution viewer with a solution model.
        /// </summary>
        /// <param name="theSolution">The solution model.</param>
        public WorkspaceViewerViewModel(SolutionModel theSolution)
        {
            Contract.Requires<ArgumentNullException>(theSolution != null);

            Model = theSolution;
            Snapshot = new SnapshotViewerViewModel();
            ActivateItem(Snapshot);
            Viewer = new WorkspaceViewerPanelViewModel(theSolution);
            ActivateItem(Viewer);
        }

        /// <summary>
        /// Gets or sets the solution view panel view model.
        /// </summary>
        public WorkspaceViewerPanelViewModel Viewer
        {
            get { return this.viewer; }
            set
            {
                this.viewer = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the solution snapshot view model.
        /// </summary>
        public SnapshotViewerViewModel Snapshot
        {
            get { return this.snapshot; }
            set
            {
                this.snapshot = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the solution model.
        /// </summary>
        public SolutionModel Model { get; private set; }

        /// <summary>
        /// Bind the solution model to the solution view model.
        /// </summary>
        /// <param name="theSolution">Solution model.</param>
        public void BindTo(SolutionModel theSolution)
        {
            Contract.Requires<ArgumentNullException>(theSolution != null);
            Reset();
            Model = theSolution;
            Viewer.BindTo(theSolution);
            Snapshot.BindTo(theSolution);
        }

        /// <summary>
        /// Reset the contents of the solution.
        /// </summary>
        public void Reset()
        {
            Snapshot.Reset();
        }

        /// <summary>
        /// Add a value.
        /// </summary>
        /// <param name="newValueViewModel">New value.</param>
        public void AddValue(ValueModel newValueViewModel)
        {
            Contract.Requires<ArgumentNullException>(newValueViewModel != null);
            Snapshot.AddValue(newValueViewModel);
        }

        /// <summary>
        /// Add a new variable visualizer.
        /// </summary>
        /// <param name="newVisualizer">New visualizer.</param>
        public void AddVisualizer(VisualizerViewerViewModel newVisualizer)
        {
            Contract.Requires<ArgumentNullException>(newVisualizer != null);
            Viewer.AddVisualizer(newVisualizer);
        }
    }
}
