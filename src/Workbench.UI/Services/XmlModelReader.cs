using System;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal class XmlModelReader
    {
        private readonly WorkspaceModel _workspace;

        internal XmlModelReader(WorkspaceModel theWorkspace)
        {
            _workspace = theWorkspace;
        }

        internal ModelModel Read(XmlNode theModelNode)
        {
            var model = _workspace.Model;
            for (var i = 0; i < theModelNode.ChildNodes.Count; i++)
            {
                var childNode = theModelNode.ChildNodes[i];
                switch (childNode.Name.ToLower())
                {
                    case "name":
                        _workspace.Model.Name = new ModelName(childNode.InnerText);
                        break;

                    case "constraints":
                        new XmlConstraintReader(model).Read(childNode.ChildNodes);
                        break;

                    case "domains":
                        new XmlSharedDomainReader(model).Read(childNode.ChildNodes);
                        break;

                    case "variables":
                        new XmlVariableReader(model).Read(childNode.ChildNodes);
                        break;

                    default:
                        throw new NotImplementedException("Unknown model node.");
                }
            }

            return model;
        }
    }
}