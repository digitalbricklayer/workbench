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
            AddRowCommand = new CommandHandler(AddRowHandler, _ => CanEditTableExecute);
            AddColumnCommand = new CommandHandler(AddColumnHandler, _ => CanEditTableExecute);
        }

        /// <summary>
        /// Gets the Solution|Edit Map command
        /// </summary>
        public ICommand AddRowCommand { get; private set; }

        /// <summary>
        /// Gets the Solution|Add Column command.
        /// </summary>
        public ICommand AddColumnCommand { get; private set; }

        private void AddColumnHandler()
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

        private void AddRowHandler()
        {
            var selectedTableVisualizers = this.workArea.GetSelectedTableVisualizers();
            if (!selectedTableVisualizers.Any()) return;
            var selectedGridVisualizer = selectedTableVisualizers.First();
            selectedGridVisualizer.AddRow(new TableRowModel());
        }

        /// <summary>
        /// Gets whether the "Table|Edit Table" menu item can be executed.
        /// </summary>
        public bool CanEditTableExecute
        {
            get { return this.workArea.GetSelectedTableVisualizers().Any(); }
        }
    }
}