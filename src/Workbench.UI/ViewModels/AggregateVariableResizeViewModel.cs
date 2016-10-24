using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the aggregate variable resize dialog box.
    /// </summary>
    public sealed class AggregateVariableResizeViewModel : Screen
    {
        /// <summary>
        /// Gets or sets the aggregate variable size.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Okay button clicked.
        /// </summary>
        public void AcceptButton()
        {
            TryClose(true);
        }
    }
}
