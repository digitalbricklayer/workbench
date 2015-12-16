using System;
using System.Windows;
using DynaApp.ViewModels;

namespace DynaApp.Views
{
    /// <summary>
    /// Interaction logic for AggregateVariableResizeWindow.xaml
    /// </summary>
    public partial class AggregateVariableResizeView : Window
    {
        public AggregateVariableResizeView()
        {
            InitializeComponent();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.variableSizeEditor.Text)) return;
            this.ViewModel.Size = Convert.ToInt32(this.variableSizeEditor.Text);
            this.DialogResult = true; 
        }

        /// <summary>
        /// Gets the dialog view model.
        /// </summary>
        public AggregateVariableResizeViewModel ViewModel
        {
            get
            {
                return (AggregateVariableResizeViewModel)this.DataContext;
            }
        }
    }
}
