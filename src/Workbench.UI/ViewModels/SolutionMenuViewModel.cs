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
        private readonly WorkAreaViewModel workArea;
        private readonly IWindowManager windowManager;

        /// <summary>
        /// Initialize the solution menu view model with default values.
        /// </summary>
        public SolutionMenuViewModel(IWindowManager theWindowManager, WorkAreaViewModel theWorkArea)
        {
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            Contract.Requires<ArgumentNullException>(theWorkArea != null);

            this.workArea = theWorkArea;
            this.windowManager = theWindowManager;
            AddChessboardCommand = IoC.Get<AddChessboardVisualizerCommand>();
            AddTableCommand = IoC.Get<AddGridVisualizerCommand>();
            EditSolutionCommand = IoC.Get<EditSolutionCommand>();
            AddRowCommand = new CommandHandler(AddRowHandler, _ => CanEditTableExecute);
            AddColumnCommand = new CommandHandler(AddColumnHandler, _ => CanEditTableExecute);
        }

        /// <summary>
        /// Gets the Solution|Add Column command.
        /// </summary>
        public ICommand AddColumnCommand { get; private set; }

        /// <summary>
        /// Gets the Insert|Table command
        /// </summary>
        public ICommand AddTableCommand { get; private set; }

        /// <summary>
        /// Gets the Insert|Chessboard command.
        /// </summary>
        public ICommand AddChessboardCommand { get; private set; }

        /// <summary>
        /// Gets the Solution|Edit Solution command.
        /// </summary>
        public ICommand EditSolutionCommand { get; private set; }

        /// <summary>
        /// Gets the Solution|Edit Map command
        /// </summary>
        public ICommand AddRowCommand { get; private set; }

        /// <summary>
        /// Gets whether the "Solution|Edit Table" menu item can be executed.
        /// </summary>
        public bool CanEditTableExecute
        {
            get { return this.workArea.GetSelectedGridVisualizers().Any(); }
        }

        private void AddColumnHandler()
        {
            var selectedGridVisualizers = this.workArea.GetSelectedGridVisualizers();
            if (!selectedGridVisualizers.Any()) return;
            var selectedGridVisualizer = selectedGridVisualizers.First();
            var columnNameEditor = new ColumnNameEditorViewModel();
            var result = this.windowManager.ShowDialog(columnNameEditor);
            if (result.GetValueOrDefault())
            {
                selectedGridVisualizer.AddColumn(new TableColumnModel(columnNameEditor.ColumnName));
            }
        }

        private void AddRowHandler()
        {
            var selectedGridVisualizers = this.workArea.GetSelectedGridVisualizers();
            if (!selectedGridVisualizers.Any()) return;
            var selectedGridVisualizer = selectedGridVisualizers.First();
            selectedGridVisualizer.AddRow(new TableRowModel());
        }
    }
}
