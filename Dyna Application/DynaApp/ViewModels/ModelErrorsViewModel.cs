using System.Collections.ObjectModel;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// Model errors view model for the errors dialog.
    /// </summary>
    public sealed class ModelErrorsViewModel
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
    }
}
