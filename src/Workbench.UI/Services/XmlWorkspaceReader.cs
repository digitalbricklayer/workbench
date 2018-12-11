using System;
using System.IO;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    public class XmlWorkspaceReader : IWorkspaceReader
    {
        public WorkspaceModel Read(string filename)
        {
			using (var inputStream = new FileStream(filename, FileMode.Open))
			{
				var workspaceDocument = new XmlDocument();
				workspaceDocument.Load(inputStream);
				var workspaceNode = workspaceDocument.FirstChild;
				if (!workspaceNode.HasChildNodes) throw new Exception("Invalid workspace file.");
				var theWorkspace = new WorkspaceModel();
				for (var i = 0; i < workspaceNode.ChildNodes.Count; i++)
				{
					var topLevelNode = workspaceNode.ChildNodes[i];
					switch (topLevelNode.Name.ToLower())
					{
						case "model":
							theWorkspace.Model = new XmlModelReader(theWorkspace).Read(topLevelNode);
							break;

						case "display":
							theWorkspace.Display = new XmlDisplayReader(theWorkspace).Read(topLevelNode);
							break;
							
						default:
							throw new NotImplementedException("Unknown top level node.");
					}
				}

				return theWorkspace;
			}
        }
    }
}
