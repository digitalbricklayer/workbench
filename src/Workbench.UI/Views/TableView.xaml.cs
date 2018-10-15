using System.Diagnostics.Contracts;
using System.Windows.Controls;
using Workbench.ViewModels;

namespace Workbench.Views
{
    /// <summary>
    /// Interaction logic for TableView.xaml
    /// </summary>
    public partial class TableView : UserControl
    {
        public TableView()
        {
            InitializeComponent();
        }

        private void OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            Contract.Assert(dataGrid != null);
            if (dataGrid.CurrentCell.Column == null) return;
            var columnIndex = dataGrid.CurrentCell.Column.DisplayIndex;
            var rowIndex = dataGrid.Items.IndexOf(dataGrid.CurrentItem);
            var tableViewModel = DataContext as TableViewModel;
            Contract.Assert(tableViewModel != null);
            tableViewModel.SelectedRow = rowIndex;
            tableViewModel.SelectedColumn = columnIndex;
        }
    }
}
