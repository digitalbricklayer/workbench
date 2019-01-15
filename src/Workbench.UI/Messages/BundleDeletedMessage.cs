using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Messages
{
    public sealed class BundleDeletedMessage : WorkspaceChangedMessage
    {
        public BundleDeletedMessage(BundleModel bundleDeleted)
        {
            Contract.Requires<ArgumentNullException>(bundleDeleted != null);
            BundleDeleted = bundleDeleted;
        }

        public BundleModel BundleDeleted { get; }
    }
}
