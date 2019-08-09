using System;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal sealed class XmlDisplayReader
    {
        private readonly WorkspaceModel _workspace;

        internal XmlDisplayReader(WorkspaceModel theWorkspace)
        {
            _workspace = theWorkspace;
        }

        internal DisplayModel Read(XmlNode displayNode)
        {
            var theDisplay = new DisplayModel(_workspace.Model);

            for (var i = 0; i < displayNode.ChildNodes.Count; i++)
            {
                var topLevelNode = displayNode.ChildNodes[i];
                switch (topLevelNode.Name.ToLower())
                {
                    case "bindings":
                        new XmlVisualizerBindingReader(theDisplay).Read(topLevelNode.ChildNodes);
                        break;

                    case "visualizers":
                        new XmlVisualizerReader(theDisplay).Read(topLevelNode.ChildNodes);
                        break;

                    default:
                        throw new NotImplementedException("Unknown top level node.");
                }
            }

            return theDisplay;
        }
    }
}
