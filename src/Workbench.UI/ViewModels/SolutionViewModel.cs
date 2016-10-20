using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution.
    /// </summary>
    public sealed class SolutionViewModel : Conductor<GraphicViewModel>.Collection.AllActive
    {
        private SolutionViewerViewModel viewer;
        private SolutionDesignerViewModel designer;

        /// <summary>
        /// Initialize the solution with a solution model.
        /// </summary>
        /// <param name="theDesigner">Solution designer.</param>
        /// <param name="theViewer">Solutuion display.</param>
        public SolutionViewModel(SolutionDesignerViewModel theDesigner, SolutionViewerViewModel theViewer)
        {
            Contract.Requires<ArgumentNullException>(theDesigner != null);
            Contract.Requires<ArgumentNullException>(theViewer != null);

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
        /// Add a new visualizer to the solution.
        /// </summary>
        /// <param name="newVisualizer">New visualizer.</param>
        public void AddVisualizer(VisualizerViewModel newVisualizer)
        {
            Contract.Requires<ArgumentNullException>(newVisualizer != null);

            Designer.AddVisualizer(newVisualizer.Designer);
            Viewer.AddVisualizer(newVisualizer.Viewer);
        }

        /// <summary>
        /// Reset the solution.
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
    }
}
