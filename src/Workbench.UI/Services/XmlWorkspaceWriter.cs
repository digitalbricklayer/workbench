using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    /// <summary>
    /// Class for writing a workspace model to XML.
    /// </summary>
    public class XmlWorkspaceWriter : IWorkspaceWriter
    {
        public void Write(string filename, WorkspaceModel theWorkspace)
        {
            var workspaceDocument = new XmlDocument();
            var workspaceRoot = workspaceDocument.CreateElement("workspace");
            workspaceDocument.AppendChild(workspaceRoot);
            new XmlModelWriter(workspaceDocument, theWorkspace.Model).Write(workspaceRoot);
            workspaceDocument.Save(filename);
        }
    }
}
