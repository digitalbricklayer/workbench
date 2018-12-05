using System;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal class XmlModelReader
    {
        private readonly WorkspaceModel _workspace;

        public XmlModelReader(WorkspaceModel theWorkspace)
        {
            _workspace = theWorkspace;
        }

        internal ModelModel Read(XmlNode theModelNode)
        {
            var modelNameAttribute = theModelNode.Attributes["name"];
            var modelName = modelNameAttribute.Value;
            _workspace.Model.Name = new ModelName(modelName);
            var model = _workspace.Model;
            for (var i = 0; i < theModelNode.ChildNodes.Count; i++)
            {
                var childNode = theModelNode.ChildNodes[i];
                switch (childNode.Name.ToLower())
                {
                    case "constraints":
                        new XmlConstraintReader(model).Read(childNode.ChildNodes);
                        break;

                    case "domains":
                        new XmlSharedDomainReader(model).Read(childNode.ChildNodes);
                        break;

                    case "variables":
                        new XmlVariableReader(_workspace).Read(childNode.ChildNodes);
                        break;

                    default:
                        throw new NotImplementedException("Unknown model node.");
                }
            }

            return model;
        }
    }
}