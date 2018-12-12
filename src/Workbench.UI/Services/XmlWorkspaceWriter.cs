using System.IO;
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
			using (var fileStream = new FileStream(filename, FileMode.Create))
			{
				var workspaceRoot = workspaceDocument.CreateElement("workspace");
				new XmlModelWriter(workspaceDocument, theWorkspace.Model).Write(workspaceRoot);
				new XmlDisplayWriter(workspaceDocument, theWorkspace.Display).Write(workspaceRoot);
				workspaceDocument.AppendChild(workspaceRoot);
				workspaceDocument.Save(fileStream);
			}
        }
    }
}
