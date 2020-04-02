using Workbench.Core.Models;

namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when a new bundle is added to the model.
    /// </summary>
    public sealed class BundleAddedMessage : WorkspaceChangedMessage
    {
        /// <summary>
        /// Initialize a new bundle added message with the new bundle.
        /// </summary>
        /// <param name="newBundle">New bundle.</param>
        public BundleAddedMessage(BundleModel newBundle)
        {
            BundleAdded = newBundle;
        }

        /// <summary>
        /// Gets the bundle that has been added.
        /// </summary>
        public BundleModel BundleAdded { get; }
    }
}
