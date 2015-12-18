using System.Windows;
using System.Windows.Input;
using Workbench.ViewModels;

namespace Workbench.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the main window view model.
        /// </summary>
        public MainWindowViewModel ViewModel
        {
            get
            {
                return (MainWindowViewModel)this.DataContext;
            }
        }

        /// <summary>
        /// Event raised to delete the selected graphics.
        /// </summary>
        private void DeleteSelectedGraphics_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewModel.Workspace.DeleteSelectedGraphics();
        }

        private void DeleteVariable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var variable = (VariableViewModel)e.Parameter;
            this.ViewModel.Workspace.Model.DeleteVariable(variable);
        }

        private void DeleteDomain_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var domain = (DomainViewModel)e.Parameter;
            this.ViewModel.Workspace.Model.DeleteDomain(domain);
        }

        private void DeleteConstraint_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var constraint = (ConstraintViewModel)e.Parameter;
            this.ViewModel.Workspace.Model.DeleteConstraint(constraint);
        }
    }
}
