using Caliburn.Micro;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for the aggregate variable resize dialog box.
    /// </summary>
    public sealed class AggregateVariableResizeViewModel : PropertyChangedBase
    {
        /// <summary>
        /// Gets or sets the aggregate variable size.
        /// </summary>
        public int Size { get; set; }
    }
}
