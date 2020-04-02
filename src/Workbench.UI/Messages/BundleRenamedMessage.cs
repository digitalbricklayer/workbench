using Workbench.Core.Models;

namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when a bundle has been renamed
    /// </summary>
    public sealed class BundleRenamedMessage : WorkspaceChangedMessage
    {
        /// <summary>
        /// Initialize a new bundle renamed message with the old name and the bundle.
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="bundleRenamed"></param>
        public BundleRenamedMessage(string oldName, BundleModel bundleRenamed)
        {
            OldName = oldName;
            BundleRenamed = bundleRenamed;
        }

        /// <summary>
        /// Gets the bundle old name.
        /// </summary>
        public string OldName { get; }

        /// <summary>
        /// Gets the bundle that was renamed.
        /// </summary>
        public BundleModel BundleRenamed { get; }
    }
}
