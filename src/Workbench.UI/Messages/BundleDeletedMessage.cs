using Workbench.Core.Models;

namespace Workbench.Messages
{
    public sealed class BundleDeletedMessage : WorkspaceChangedMessage
    {
        public BundleDeletedMessage(BundleModel bundleDeleted)
        {
            BundleDeleted = bundleDeleted;
        }

        public BundleModel BundleDeleted { get; }
    }
}
