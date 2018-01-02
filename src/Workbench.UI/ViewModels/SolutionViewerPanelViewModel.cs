using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution viewer panel.
    /// </summary>
    public sealed class SolutionViewerPanelViewModel : Conductor<VisualizerViewerViewModel>.Collection.AllActive
    {
        /// <summary>
        /// Initialize the solution viewer with a solution model.
        /// </summary>
        /// <param name="theSolution">The solution model.</param>
        public SolutionViewerPanelViewModel(SolutionModel theSolution)
        {
            Contract.Requires<ArgumentNullException>(theSolution != null);

            Model = theSolution;
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
            Model = theSolution;
            UpdateViewers();
        }

        /// <summary>
        /// Add a new variable visualizer.
        /// </summary>
        /// <param name="newVisualizer">New visualizer.</param>
        public void AddVisualizer(VisualizerViewerViewModel newVisualizer)
        {
            Contract.Requires<ArgumentNullException>(newVisualizer != null);
            ActivateItem(newVisualizer);
        }

        private void UpdateViewers()
        {
            foreach (var viewer in Items)
            {
                viewer.Update();
            }
        }
    }
}