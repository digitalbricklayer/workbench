using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution.
    /// </summary>
    public sealed class SolutionViewModel : Conductor<VisualizerViewModel>.Collection.AllActive
    {
        private SolutionViewerViewModel viewer;
        private SolutionDesignerViewModel designer;
        private readonly WorkspaceViewModel workspace;
        private SolutionDesignerViewModel solutionDesigner;
        private SolutionViewerViewModel solutionViewer;

        /// <summary>
        /// Initialize the solution with the workspace, a solution designer and solution viewer.
        /// </summary>
        /// <param name="theWorkspace">Workspace.</param>
        /// <param name="theDesigner">Solution designer.</param>
        /// <param name="theViewer">Solutuion display.</param>
        public SolutionViewModel(WorkspaceViewModel theWorkspace, SolutionDesignerViewModel theDesigner, SolutionViewerViewModel theViewer)
        {
            Contract.Requires<ArgumentNullException>(theWorkspace != null);
            Contract.Requires<ArgumentNullException>(theDesigner != null);
            Contract.Requires<ArgumentNullException>(theViewer != null);

            ChessboardVisualizers = new List<ChessboardVisualizerViewModel>();
            MapVisualizers = new List<GridVisualizerViewModel>();
            this.workspace = theWorkspace;
            Designer = theDesigner;
            Viewer = theViewer;
            Model = Viewer.Model;
        }

        /// <summary>
        /// Gets or sets the solution designer.
        /// </summary>
        public SolutionDesignerViewModel Designer
        {
            get
            {
                return this.designer;
            }
            set
            {
                this.designer = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the solution viewer.
        /// </summary>
        public SolutionViewerViewModel Viewer
        {
            get
            {
                return this.viewer;
            }
            set
            {
                this.viewer = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the solution model.
        /// </summary>
        public SolutionModel Model { get; set; }

        /// <summary>
        /// Gets all chessboard visualizers.
        /// </summary>
        public IList<ChessboardVisualizerViewModel> ChessboardVisualizers { get; private set; }

        /// <summary>
        /// Gets all map visualizers.
        /// </summary>
        public IList<GridVisualizerViewModel> MapVisualizers { get; private set; }

        /// <summary>
        /// Add a new chessboard visualizer to the workspace.
        /// </summary>
        /// <param name="newVisualizer">New chessboard visualizer.</param>
        public void AddChessboardVisualizer(ChessboardVisualizerViewModel newVisualizer)
        {
            Contract.Requires<ArgumentNullException>(newVisualizer != null);
            AddVisualizer(newVisualizer);
            ChessboardVisualizers.Add(newVisualizer);
        }

        /// <summary>
        /// Add a new map visualizer to the workspace.
        /// </summary>
        /// <param name="newVisualizer">New map visualizer.</param>
        public void AddMapVisualizer(GridVisualizerViewModel newVisualizer)
        {
            Contract.Requires<ArgumentNullException>(newVisualizer != null);
            AddVisualizer(newVisualizer);
            MapVisualizers.Add(newVisualizer);
        }

        /// <summary>
        /// Reset the current solution.
        /// </summary>
        public void Reset()
        {
            Viewer.Reset();
        }

        public void UnbindAll()
        {
            Viewer.UnbindAll();
        }

        public void BindTo(List<ValueModel> newValues)
        {
            Viewer.BindTo(newValues);
        }

        public IReadOnlyCollection<GridVisualizerViewModel> GetSelectedMapVisualizers()
        {
            if (this.workspace.SelectedDisplayMode == "Designer")
            {
                return MapVisualizers.Where(_ => _.Designer.IsSelected)
                                     .ToList();
            }

            return MapVisualizers.Where(_ => _.Viewer.IsSelected)
                                 .ToList();
        }

        /// <summary>
        /// Add a new visualizer to the solution.
        /// </summary>
        /// <param name="newVisualizer">New visualizer.</param>
        private void AddVisualizer(VisualizerViewModel newVisualizer)
        {
            Contract.Requires<ArgumentNullException>(newVisualizer != null);

            Designer.AddVisualizer(newVisualizer.Designer);
            Viewer.AddVisualizer(newVisualizer.Viewer);
            ActivateItem(newVisualizer);
        }
    }
}
