using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Table tab for editing a table with input data or as an output display format.
    /// </summary>
    public sealed class TableTabViewModel : Conductor<IScreen>.Collection.AllActive, IWorkspaceTabViewModel
    {
        private TableViewModel _table;
        private string _name;
        private string _title;
        private readonly IWindowManager _windowManager;

        public TableTabViewModel(TableTabModel theTableModel, IEventAggregator theEventAggregator, IWindowManager theWindowManager)
        {
            Contract.Requires<ArgumentNullException>(theTableModel != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);

            _windowManager = theWindowManager;
            _name = DisplayName = "table1";
            _title = "table1";
            Model = theTableModel;
            Table = new TableViewModel(theTableModel.Table);
        }

        /// <summary>
        /// Gets whether the tab can be manually closed by the user.
        /// </summary>
        public bool CloseTabIsVisible => true;

        /// <summary>
        /// Gets the table tab model.
        /// </summary>
        public TableTabModel Model { get; }

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the table title.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                NotifyOfPropertyChange();
            }
        }

        public int? SelectedRow => Table.SelectedRow;

        /// <summary>
        /// Gets the selected column.
        /// </summary>
        public int? SelectedColumn => Table.SelectedColumn;

        /// <summary>
        /// Gets or sets the table view model.
        /// </summary>
        public TableViewModel Table
        {
            get => _table;
            set
            {
                _table = value;
                NotifyOfPropertyChange();
            }
        }

        public void AddColumnBefore(int selectedColumnIndex, TableColumnModel newColumn)
        {
            Contract.Requires<ArgumentNullException>(newColumn != null);
            Table.AddColumnBefore(selectedColumnIndex, newColumn);
        }

        public void AddColumnAfter(int selectedColumnIndex, TableColumnModel newColumn)
        {
            Contract.Requires<ArgumentNullException>(newColumn != null);
            Table.AddColumnAfter(selectedColumnIndex, newColumn);
        }

        public void AddRowBefore(int selectedRowIndex, TableRowModel newRow)
        {
            Contract.Requires<ArgumentNullException>(newRow != null);
            Contract.Assume(Table != null);
            Table.AddRowBefore(selectedRowIndex, newRow);
        }

        public void AddRowAfter(int selectedRowIndex, TableRowModel newRow)
        {
            Contract.Requires<ArgumentNullException>(newRow != null);
            Contract.Assume(Table != null);
            Table.AddRowAfter(selectedRowIndex, newRow);
        }

        public void Resize(int columns, int rows)
        {
            Contract.Assume(Table != null);
            Table.Resize(columns, rows);
        }

        public void EditTitle()
        {
            var titleEditor = new TableTitleEditorViewModel();
            titleEditor.TableTitle = Title;
            var status = _windowManager.ShowDialog(titleEditor);
            if (status.GetValueOrDefault()) return;
            Title = titleEditor.TableTitle;
        }

        public void EditName()
        {
            var nameEditor = new TableNameEditorViewModel();
            nameEditor.TableName = Name;
            var status = _windowManager.ShowDialog(nameEditor);
            if (status.GetValueOrDefault()) return;
            Name = nameEditor.TableName;
        }
    }
}
