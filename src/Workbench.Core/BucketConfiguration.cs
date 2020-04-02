using Workbench.Core.Models;

namespace Workbench.Core
{
    public sealed class BucketConfiguration
    {
        private string _name;
        private string _bundleName;
        private int _size;
        private readonly WorkspaceModel _workspace;

        public BucketConfiguration(WorkspaceModel workspace)
        {
            _workspace = workspace;
            _name = string.Empty;
            _bundleName = string.Empty;
            _size = 1;
        }

        public BucketConfiguration WithName(string name)
        {
            _name = name;
            return this;
        }

        public BucketConfiguration WithSize(int size)
        {
            _size = size;
            return this;
        }

        public BucketConfiguration WithContents(string bundleName)
        {
            _bundleName = bundleName;
            return this;
        }

        public BucketVariableModel Build()
        {
            var bundle = _workspace.Model.GetBundleByName(_bundleName);
            return new BucketVariableModel(_workspace, new ModelName(_name), _size, bundle);
        }
    }
}
