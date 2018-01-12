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
        private readonly WorkAreaViewModel workspace;
        private readonly IWindowManager windowManager;

        /// <summary>
        /// Initialize the solution menu view model with default values.
        /// </summary>
        public SolutionMenuViewModel(IWindowManager theWindowManager, WorkAreaViewModel theWorkspace)
        {
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            Contract.Requires<ArgumentNullException>(theWorkspace != null);

            this.workspace = theWorkspace;
            this.windowManager = theWindowManager;
            AddChessboardVisualizerCommand = IoC.Get<AddChessboardVisualizerCommand>();
            AddGridVisualizerCommand = IoC.Get<AddGridVisualizerCommand>();
            EditSolutionCommand = IoC.Get<EditSolutionCommand>();
            AddRowCommand = new CommandHandler(AddRowHandler, _ => CanEditGridExecute);
            AddColumnCommand = new CommandHandler(AddColumnHandler, _ => CanEditGridExecute);
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
        public ICommand AddRowCommand { get; private set; }

        /// <summary>
        /// Gets whether the "Solution|Edit Grid" menu item can be executed.
        /// </summary>
        public bool CanEditGridExecute
        {
            get { return this.workspace.Solution.GetSelectedGridVisualizers().Any(); }
        }

        private void AddColumnHandler()
        {
            var selectedGridVisualizers = this.workspace.Solution.GetSelectedGridVisualizers();
            if (!selectedGridVisualizers.Any()) return;
            var selectedGridVisualizer = selectedGridVisualizers.First();
            var columnNameEditor = new ColumnNameEditorViewModel();
            var result = this.windowManager.ShowDialog(columnNameEditor);
            if (result.GetValueOrDefault())
            {
                selectedGridVisualizer.AddColumn(new GridColumnModel(columnNameEditor.ColumnName));
            }
        }

        private void AddRowHandler()
        {
            var selectedGridVisualizers = this.workspace.Solution.GetSelectedGridVisualizers();
            if (!selectedGridVisualizers.Any()) return;
            var selectedGridVisualizer = selectedGridVisualizers.First();
            selectedGridVisualizer.AddRow(new GridRowModel());
        }
    }
}
