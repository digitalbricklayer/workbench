using System.Collections.ObjectModel;
using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Model errors view model for the errors dialog.
    /// </summary>
    public sealed class ModelErrorsViewModel : Screen
    {
        /// <summary>
        /// Initialize with default values.
        /// </summary>
        public ModelErrorsViewModel()
        {
            this.Errors = new ObservableCollection<ModelErrorViewModel>();
        }

        /// <summary>
        /// Gets the model errors.
        /// </summary>
        public ObservableCollection<ModelErrorViewModel> Errors { get; private set; }

        /// <summary>
        /// Close button clicked.
        /// </summary>
        public void CloseButton()
        {
            this.TryClose(true);
        }
    }
}
