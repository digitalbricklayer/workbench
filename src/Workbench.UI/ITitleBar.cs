namespace Workbench
{
    /// <summary>
    /// Contract for the application title bar.
    /// </summary>
    public interface ITitleBar
    {
        /// <summary>
        /// Gets or sets the title bar text.
        /// </summary>
        string Title { get; set; }
    }
}