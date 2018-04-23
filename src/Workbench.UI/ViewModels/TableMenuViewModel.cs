using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public sealed class TableMenuViewModel
    {
        private readonly WorkAreaViewModel workArea;
        private readonly IWindowManager windowManager;

        public TableMenuViewModel(IWindowManager theWindowManager, WorkAreaViewModel theWorkArea)
        {
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            Contract.Requires<ArgumentNullException>(theWorkArea != null);

            this.workArea = theWorkArea;
            this.windowManager = theWindowManager;
            AddRowAfterCommand = new CommandHandler(AddRowAfter, _ => CanEditTableExecute);
            AddColumnAfterCommand = new CommandHandler(AddColumnAfter, _ => CanEditTableExecute);
            AddRowBeforeCommand = new CommandHandler(AddRowBefore, _ => CanEditTableExecute);
            AddColumnBeforeCommand = new CommandHandler(AddColumnBefore, _ => CanEditTableExecute);
            DeleteSelectedRowCommand = new CommandHandler(DeleteSelectedRow, _ => CanEditTableExecute);
            DeleteSelectedColumnCommand = new CommandHandler(DeleteSelectedColumn, _ => CanEditTableExecute);
        }

        /// <summary>
        /// Gets the Table|Add Row Before command
        /// </summary>
        public ICommand AddRowBeforeCommand { get; }

        /// <summary>
        /// Gets the Table|Add Row After command
        /// </summary>
        public ICommand AddRowAfterCommand { get; }

        /// <summary>
        /// Gets the Table|Add Column Before command.
        /// </summary>
        public ICommand AddColumnBeforeCommand { get; }

        /// <summary>
        /// Gets the Table|Add Column After command.
        /// </summary>
        public ICommand AddColumnAfterCommand { get; }

        /// <summary>
        /// Gets the Table|Delete Selected Row command.
        /// </summary>
        public ICommand DeleteSelectedRowCommand { get; set; }

        /// <summary>
        /// Gets the Table|Delete Selected Column command.
        /// </summary>
        public ICommand DeleteSelectedColumnCommand { get; set; }

        /// <summary>
        /// Gets whether the "Table|Edit Table" menu item can be executed.
        /// </summary>
        public bool CanEditTableExecute
        {
            get { return this.workArea.GetSelectedTableVisualizers().Any(); }
        }

        private void AddColumnAfter()
        {
            var selectedTableVisualizers = this.workArea.GetSelectedTableVisualizers();
            if (!selectedTableVisualizers.Any()) return;
            var selectedTableVisualizer = selectedTableVisualizers.First();
            var columnNameEditor = new ColumnNameEditorViewModel();
            var result = this.windowManager.ShowDialog(columnNameEditor);
            if (result.GetValueOrDefault())
            {
                selectedTableVisualizer.AddColumn(new TableColumnModel(columnNameEditor.ColumnName));
            }
        }

        private void AddRowAfter()
        {
            var selectedTableVisualizers = this.workArea.GetSelectedTableVisualizers();
            Contract.Assert(selectedTableVisualizers.Any());
            var selectedTableVisualizer = selectedTableVisualizers.First();
            selectedTableVisualizer.AddRow(new TableRowModel());
        }

        private void AddColumnBefore()
        {
            var selectedTableVisualizers = this.workArea.GetSelectedTableVisualizers();
            Contract.Assert(selectedTableVisualizers.Any());
            var selectedTableVisualizer = selectedTableVisualizers.First();
            var columnNameEditor = new ColumnNameEditorViewModel();
            var result = this.windowManager.ShowDialog(columnNameEditor);
            if (result.GetValueOrDefault())
            {
                selectedTableVisualizer.AddColumn(new TableColumnModel(columnNameEditor.ColumnName));
            }
        }

        private void AddRowBefore()
        {
            var selectedTableVisualizers = this.workArea.GetSelectedTableVisualizers();
            if (!selectedTableVisualizers.Any()) return;
            var selectedTableVisualizer = selectedTableVisualizers.First();
            selectedTableVisualizer.AddRow(new TableRowModel());
        }

        private void DeleteSelectedColumn()
        {
            var selectedTableVisualizers = this.workArea.GetSelectedTableVisualizers();
            if (!selectedTableVisualizers.Any()) return;
            var selectedTableVisualizer = selectedTableVisualizers.First();
            selectedTableVisualizer.DeleteColumnSelected();
        }

        private void DeleteSelectedRow()
        {
            var selectedTableVisualizers = this.workArea.GetSelectedTableVisualizers();
            if (!selectedTableVisualizers.Any()) return;
            var selectedTableVisualizer = selectedTableVisualizers.First();
            selectedTableVisualizer.DeleteRowSelected();
        }
    }
}