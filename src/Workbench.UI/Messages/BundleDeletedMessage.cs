using Workbench.Core.Models;

namespace Workbench.Messages
{
    public sealed class BundleDeletedMessage : WorkspaceChangedMessage
    {
        public BundleDeletedMessage(BundleTemplateModel bundleDeleted)
        {
            BundleDeleted = bundleDeleted;
        }

        public BundleTemplateModel BundleDeleted { get; }
    }
}
