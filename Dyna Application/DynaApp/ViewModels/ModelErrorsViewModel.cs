using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for all model errors.
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
