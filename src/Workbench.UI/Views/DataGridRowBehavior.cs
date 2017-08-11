using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Workbench.Views
{
    public class DataGridRowBehavior
    {
        private static IDictionary<DependencyObject, ObservableCollection<DataGridRow>> rowMap =
            new Dictionary<DependencyObject, ObservableCollection<DataGridRow>>();

        public static readonly DependencyProperty BindableRowsProperty =
            DependencyProperty.RegisterAttached("BindableRows",
                typeof(ObservableCollection<DataGridRow>),
                typeof(DataGridRowBehavior),
                new UIPropertyMetadata(null, BindableRowsPropertyChanged));

        private static void BindableRowsPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = source as DataGrid;
            var rows = e.NewValue as ObservableCollection<DataGridRow>;
            if (rows == null) return;
            rowMap[source] = rows;
            var dt = new DataTable();
            dt.Columns.Add("attr1", typeof(string));
            dt.Columns.Add("attr2", typeof(string));
            dt.Columns.Add("attr3", typeof(string));
            dt.Rows.Add();
#if false
            rows.Clear();
            foreach (DataGridRow row in rows)
            {
                rows.Add(row);
            }
            dataGrid.DataSource = dt;
#endif
            rows.CollectionChanged += (sender, e2) =>
            {
                NotifyCollectionChangedEventArgs ne = e2 as NotifyCollectionChangedEventArgs;
                if (ne.Action == NotifyCollectionChangedAction.Reset)
                {
                    rows.Clear();
                    foreach (DataGridRow row in ne.NewItems)
                    {
                        rows.Add(row);
                    }
                }
                else if (ne.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (DataGridRow row in ne.NewItems)
                    {
                        rows.Add(row);
                    }
                }
                else if (ne.Action == NotifyCollectionChangedAction.Move)
                {
                    rows.Move(ne.OldStartingIndex, ne.NewStartingIndex);
                }
                else if (ne.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (DataGridRow row in ne.OldItems)
                    {
                        rows.Remove(row);
                    }
                }
                else if (ne.Action == NotifyCollectionChangedAction.Replace)
                {
                    rows[ne.NewStartingIndex] = ne.NewItems[0] as DataGridRow;
                }
            };
        }

        public static void SetBindableRows(DependencyObject element, ObservableCollection<DataGridRow> value)
        {
            element.SetValue(BindableRowsProperty, value);
        }

        public static ObservableCollection<DataGridRow> GetBindableRows(DependencyObject element)
        {
            return (ObservableCollection<DataGridRow>)element.GetValue(BindableRowsProperty);
        }
    }
}
