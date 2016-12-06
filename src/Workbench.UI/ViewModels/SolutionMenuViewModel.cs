using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Commands;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution main menu.
    /// </summary>
    public class SolutionMenuViewModel
    {
        private readonly WorkspaceViewModel workspace;
        private readonly IWindowManager windowManager;

        /// <summary>
        /// Initialize the solution menu view model with default values.
        /// </summary>
        public SolutionMenuViewModel(IWindowManager theWindowManager, WorkspaceViewModel theWorkspace)
        {
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            Contract.Requires<ArgumentNullException>(theWorkspace != null);

            this.workspace = theWorkspace;
            this.windowManager = theWindowManager;
            AddChessboardVisualizerCommand = IoC.Get<AddChessboardVisualizerCommand>();
            AddGridVisualizerCommand = IoC.Get<AddMapVisualizerCommand>();
            EditGridVisualizerCommand = new CommandHandler(EditGridHandler, _ => CanEditGridExecute);
            EditSolutionCommand = IoC.Get<EditSolutionCommand>();
            AddColumnCommand = new CommandHandler(AddColumnHandler, _ => CanAddColumnExecute);
        }

        /// <summary>
        /// Gets the Solution|Add Column command.
        /// </summary>
        public ICommand AddColumnCommand { get; private set; }

        /// <summary>
        /// Gets the Solution|Add Map command
        /// </summary>
        public ICommand AddGridVisualizerCommand { get; private set; }

        /// <summary>
        /// Gets the Solution|Add Chessboard command.
        /// </summary>
        public ICommand AddChessboardVisualizerCommand { get; private set; }

        /// <summary>
        /// Gets the Solution|Edit Solution command.
        /// </summary>
        public ICommand EditSolutionCommand { get; private set; }

        /// <summary>
        /// Gets the Solution|Edit Map command
        /// </summary>
        public ICommand EditGridVisualizerCommand { get; private set; }

        /// <summary>
        /// Gets whether the "Solution|Add Column" menu item can be executed.
        /// </summary>
        public bool CanAddColumnExecute
        {
            get { return IsGridVisualizerSelected(); }
        }

        /// <summary>
        /// Gets whether the "Solution|Edit Grid" menu item can be executed.
        /// </summary>
        public bool CanEditGridExecute
        {
            get { return IsGridVisualizerSelected(); }
        }

        private bool IsGridVisualizerSelected()
        {
            return this.workspace.Solution.GetSelectedGridVisualizers().Any();
        }

        private void AddColumnHandler()
        {
            var selectedMapVisualizers = this.workspace.Solution.GetSelectedGridVisualizers();
            if (!selectedMapVisualizers.Any()) return;
            var selectedMapVisualizer = selectedMapVisualizers.First();
            var columnNameEditor = new ColumnNameEditorViewModel();
            var result = this.windowManager.ShowDialog(columnNameEditor);
            if (result.GetValueOrDefault())
            {
                selectedMapVisualizer.AddColumn(new GridColumnModel(columnNameEditor.ColumnName));
            }
        }

        private void EditGridHandler()
        {
            var selectedMapVisualizers = this.workspace.Solution.GetSelectedGridVisualizers();
            if (!selectedMapVisualizers.Any()) return;
            var mapEditorViewModel = new GridEditorViewModel();
            //mapEditorViewModel.BackgroundImagePath = selectedMapVisualizers.First().Model.Model.BackgroundImagePath;
            var showDialogResult = this.windowManager.ShowDialog(mapEditorViewModel);
            if (showDialogResult.GetValueOrDefault())
            {
                foreach (var mapVisualizer in selectedMapVisualizers)
                {
//                    mapVisualizer.Model.Model.BackgroundImagePath = mapEditorViewModel.BackgroundImagePath;
                }
            }

        }
    }
}
