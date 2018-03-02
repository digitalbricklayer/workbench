using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench
{
    internal class DefaultDataService : IDataService
    {
        public WorkspaceModel Open(string file)
        {
            return null;
        }

        public void Save(string file)
        {
        }

        public WorkspaceModel GetWorkspace()
        {
            return null;
        }

        public VariableGraphicModel GetVariableByName(string variableName)
        {
            return null;
        }
    }
}