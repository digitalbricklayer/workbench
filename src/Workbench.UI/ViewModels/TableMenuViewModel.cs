using System.Diagnostics;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public sealed class TableMenuViewModel
    {
        private readonly WorkspaceViewModel _workspace;
        private readonly IWindowManager _windowManager;

        public TableMenuViewModel(IWindowManager theWindowManager, WorkspaceViewModel theWorkspace)
        {
            _workspace = theWorkspace;
            _windowManager = theWindowManager;
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
        public bool CanEditTableExecute => _workspace.ActiveItem is TableTabViewModel;

        private void AddColumnAfter()
        {
            var selectedTableTab = GetSelectedTableTab();
            Debug.Assert(selectedTableTab.SelectedColumn != null);
            var columnNameEditor = new ColumnNameEditorViewModel();
            var result = this._windowManager.ShowDialog(columnNameEditor);
            if (result.GetValueOrDefault())
            {
                selectedTableTab.AddColumnAfter(selectedTableTab.SelectedColumn.Value, new TableColumnModel(columnNameEditor.ColumnName));
            }
        }

        private void AddColumnBefore()
        {
            var selectedTableTab = GetSelectedTableTab();
            Debug.Assert(selectedTableTab.SelectedColumn != null);
            var columnNameEditor = new ColumnNameEditorViewModel();
            var result = this._windowManager.ShowDialog(columnNameEditor);
            if (result.GetValueOrDefault())
            {
                selectedTableTab.AddColumnBefore(selectedTableTab.SelectedColumn.Value, new TableColumnModel(columnNameEditor.ColumnName));
            }
        }

        private void AddRowAfter()
        {
            var selectedTableTab = GetSelectedTableTab();
            Debug.Assert(selectedTableTab.SelectedRow != null);
            selectedTableTab.AddRowAfter(selectedTableTab.SelectedRow.Value, new TableRowModel());
        }

        private void AddRowBefore()
        {
            var selectedTableTab = GetSelectedTableTab();
            Debug.Assert(selectedTableTab.SelectedRow != null);
            selectedTableTab.AddRowBefore(selectedTableTab.SelectedRow.Value, new TableRowModel());
        }

        private void DeleteSelectedColumn()
        {
            var selectedTableTab = GetSelectedTableTab();
            selectedTableTab.DeleteColumnSelected();
        }

        private void DeleteSelectedRow()
        {
            var selectedTableTab = GetSelectedTableTab();
            selectedTableTab.DeleteRowSelected();
        }

        private TableTabViewModel GetSelectedTableTab()
        {
            return _workspace.ActiveItem as TableTabViewModel;
        }
    }
}