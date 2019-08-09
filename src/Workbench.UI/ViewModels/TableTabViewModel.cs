using System;
using System.ComponentModel;
using System.Diagnostics;
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
        private string _text;
        private TableDetailsViewModel _details;

        public TableTabViewModel(TableTabModel theTableModel, IEventAggregator theEventAggregator, IWindowManager theWindowManager)
        {
            _windowManager = theWindowManager;
            TabText = Name = DisplayName = theTableModel.Name;
            Title = theTableModel.Title.Text;
            Model = theTableModel;
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
        /// Gets or sets the table details.
        /// </summary>
        public TableDetailsViewModel Details
        {
            get => _details;
            set
            {
                _details = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                TabText = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the tab text.
        /// </summary>
        public string TabText
        {
            get => _text;
            set
            {
                _text= value;
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
            Table.AddColumnBefore(selectedColumnIndex, newColumn);
        }

        public void AddColumnAfter(int selectedColumnIndex, TableColumnModel newColumn)
        {
            Table.AddColumnAfter(selectedColumnIndex, newColumn);
        }

        public void AddRowBefore(int selectedRowIndex, TableRowModel newRow)
        {
            Debug.Assert(Table != null);
            Table.AddRowBefore(selectedRowIndex, newRow);
        }

        public void AddRowAfter(int selectedRowIndex, TableRowModel newRow)
        {
            Debug.Assert(Table != null);
            Table.AddRowAfter(selectedRowIndex, newRow);
        }

        /// <summary>
        /// Delete the currently selected column.
        /// </summary>
        public void DeleteColumnSelected()
        {
            Table.DeleteColumnSelected();
        }

        /// <summary>
        /// Delete the currently selected row.
        /// </summary>
        public void DeleteRowSelected()
        {
            Table.DeleteRowSelected();
        }

        public void Resize(int columns, int rows)
        {
            Debug.Assert(Table != null);
            Table.Resize(columns, rows);
        }

        public void EditTitle()
        {
            var titleEditor = new TabTitleEditorViewModel();
            titleEditor.TabTitle = Title;
            var status = _windowManager.ShowDialog(titleEditor);
            if (status.HasValue && !status.Value) return;
            Title = titleEditor.TabTitle;
        }

        public void EditName()
        {
            var nameEditor = new TabNameEditorViewModel();
            nameEditor.TabName = Name;
            var status = _windowManager.ShowDialog(nameEditor);
            if (status.HasValue && !status.Value) return;
            Name = nameEditor.TabName;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Table = new TableViewModel(Model.Table);
            Details = new TableDetailsViewModel(_windowManager);
            Items.AddRange(new IScreen[] { Table, Details });
            Table.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedColumn":
                    ChangeCellDetails();
                    break;

                case "SelectedRow":
                    ChangeCellDetails();
                    break;
            }
        }

        private void ChangeCellDetails()
        {
            if (Table.SelectedRow == null || Table.SelectedColumn == null) return;
            var selectedCell = Model.Table.GetCellBy(Table.SelectedRow.Value + 1, Table.SelectedColumn.Value + 1);
            Details = new TableDetailsViewModel(selectedCell, _windowManager);
            var selectedColumn = Model.Table.GetColumnAt(Table.SelectedColumn.Value + 1);
            Details.Column = selectedColumn.Name;
            Details.Row = Convert.ToString(Table.SelectedRow.Value + 1);
        }

        public void UpdateFromModel()
        {
            Table.UpdateFromModel();
        }
    }
}
