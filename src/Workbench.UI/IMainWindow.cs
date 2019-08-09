namespace Workbench
{
    public interface IMainWindow
    {
        /// <summary>
        /// Gets or sets the shell.
        /// </summary>
        IShell Shell { get; set; }

        /// <summary>
        /// Gets or sets the title bar.
        /// </summary>
        ITitleBar TitleBar { get; set; }
    }
}