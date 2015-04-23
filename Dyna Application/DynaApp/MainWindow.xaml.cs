using System.Windows;
using System.Windows.Input;

namespace DynaApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Exit the application.
        /// </summary>
        void FileCloseCommand(object sender, ExecutedRoutedEventArgs args)
        {
            this.Close();
        }
    }
}
