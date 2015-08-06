using System.Windows;
using System.Windows.Input;

namespace DynaApp.Views
{
    /// <summary>
    /// Interaction logic for ModelErrorsWindow.xaml
    /// </summary>
    public partial class ModelErrorsWindow : Window
    {
        public ModelErrorsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handler for the close button.
        /// </summary>
        /// <param name="sender">Close button.</param>
        /// <param name="e">Event arguments.</param>
        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
