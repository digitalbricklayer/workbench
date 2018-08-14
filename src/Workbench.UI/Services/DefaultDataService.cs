using Workbench.Core.Models;

namespace Workbench.Services
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

        public VariableModel GetVariableByName(string variableName)
        {
            return null;
        }
    }
}