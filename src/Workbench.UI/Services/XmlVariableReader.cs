using System;
using System.Diagnostics.Contracts;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal class XmlVariableReader
    {
        private readonly ModelModel _model;
        private readonly WorkspaceModel _workspace;

        internal XmlVariableReader(WorkspaceModel theWorkspace)
        {
            Contract.Requires<ArgumentNullException>(theWorkspace != null);
            _workspace = theWorkspace;
            _model = _workspace.Model;
        }

        internal void Read(XmlNodeList variableNodeList)
        {
            for (var i = 0; i < variableNodeList.Count; i++)
            {
                var constraintNode = variableNodeList[i];
                switch (constraintNode.Name)
                {
                    case "aggregate-variable":
                        ReadAggregateVariable(constraintNode);
                        break;

                    case "singleton-variable":
                        ReadSingletonVariable(constraintNode);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void ReadSingletonVariable(XmlNode constraintNode)
        {
            var constraintIdAttribute = constraintNode.Attributes["id"];
            var constraintId = constraintIdAttribute.Value;
            var constraintNameAttribute = constraintNode.Attributes["name"];
            var constraintName = constraintNameAttribute.Value;
            var domainExpression = string.Empty;
            for (var i = 0; i < constraintNode.ChildNodes.Count; i++)
            {
                var childNode = constraintNode.ChildNodes[i];
                switch (childNode.Name)
                {
                    case "domain":
                        domainExpression = childNode.InnerText;
                        break;
                }
            }

            var variableModel = new SingletonVariableModel(_model, new ModelName(constraintName), new InlineDomainModel(domainExpression));
            variableModel.Id = Convert.ToInt32(constraintId);
            _model.AddVariable(variableModel);
        }

        private void ReadAggregateVariable(XmlNode constraintNode)
        {
            var constraintIdAttribute = constraintNode.Attributes["id"];
            var constraintId = constraintIdAttribute.Value;
            var constraintNameAttribute = constraintNode.Attributes["name"];
            var constraintName = constraintNameAttribute.Value;
            var constraintSizeAttribute = constraintNode.Attributes["size"];
            var constraintSize = constraintSizeAttribute.Value;
            var domainExpression = string.Empty;
            for (var i = 0; i < constraintNode.ChildNodes.Count; i++)
            {
                var childNode = constraintNode.ChildNodes[i];
                switch (childNode.Name)
                {
                    case "domain":
                        domainExpression = childNode.InnerText;
                        break;
                }
            }

            var variableModel = new AggregateVariableModel(_model.Workspace, new ModelName(constraintName), Convert.ToInt32(constraintSize), new InlineDomainModel(domainExpression));
            variableModel.Id = Convert.ToInt32(constraintId);
            _model.AddVariable(variableModel);
        }
    }
}