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
            menuFileResolve.Click += new RoutedEventHandler(MenuFileResolve_Click);
        }

        /// <summary>
        /// Exit the application.
        /// </summary>
        void FileCloseCommand(object sender, ExecutedRoutedEventArgs args)
        {
            this.Close();
        }

        /// <summary>
        /// Resolve the problem contained within the workspace.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuFileResolve_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
