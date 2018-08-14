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

        public TableTabViewModel(TableTabModel theTableModel, IEventAggregator theEventAggregator)
        {
            Contract.Requires<ArgumentNullException>(theTableModel != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);

            DisplayName = "Table1";
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

        public void AddColumn(TableColumnModel newColumn)
        {
            Contract.Requires<ArgumentNullException>(newColumn != null);
            Table.AddColumn(newColumn);
        }

        public void Resize(int columns, int rows)
        {
            Contract.Assume(Table != null);
            Table.Resize(columns, rows);
        }

        public void AddRow(TableRowModel newRow)
        {
            Contract.Requires<ArgumentNullException>(newRow != null);
            Contract.Assume(Table != null);
            Table.AddRow(newRow);
        }
    }
}

