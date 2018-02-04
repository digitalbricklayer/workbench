namespace Workbench.Core.Models
{
    public sealed class VisualizerTitle
    {
        public VisualizerTitle(string theText)
        {
            Text = theText;
        }

        public VisualizerTitle()
        {
            Text = string.Empty;
        }

        public string Text { get; set; }
    }
}