using System;

namespace Workbench.Core.Models
{
    [Serializable]
    public sealed class WorkspaceTabTitle
    {
        public WorkspaceTabTitle(string theText)
        {
            Text = theText;
        }

        public WorkspaceTabTitle()
        {
            Text = string.Empty;
        }

        public string Text { get; set; }
    }
}