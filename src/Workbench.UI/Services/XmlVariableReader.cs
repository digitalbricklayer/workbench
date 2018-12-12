using System;
using System.Diagnostics.Contracts;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal class XmlVariableReader
    {
        private readonly ModelModel _model;

        internal XmlVariableReader(ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            _model = theModel;
        }

        internal void Read(XmlNodeList variableNodeList)
        {
            for (var i = 0; i < variableNodeList.Count; i++)
            {
                var variableNode = variableNodeList[i];
                switch (variableNode.Name)
                {
                    case "aggregate-variable":
                        ReadAggregateVariable(variableNode);
                        break;

                    case "singleton-variable":
                        ReadSingletonVariable(variableNode);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void ReadSingletonVariable(XmlNode variableNode)
        {
            var variableIdAttribute = variableNode.Attributes["id"];
            var variableId = variableIdAttribute.Value;
            var variableName = string.Empty;
            var domainExpression = string.Empty;
            for (var i = 0; i < variableNode.ChildNodes.Count; i++)
            {
                var childNode = variableNode.ChildNodes[i];
                switch (childNode.Name)
                {
                    case "name":
                        variableName = childNode.InnerText;
                        break;

                    case "domain":
                        domainExpression = childNode.InnerText;
                        break;
                }
            }

            var variableModel = new SingletonVariableModel(_model, new ModelName(variableName), new InlineDomainModel(domainExpression));
            variableModel.Id = Convert.ToInt32(variableId);
            _model.AddVariable(variableModel);
        }

        private void ReadAggregateVariable(XmlNode variableNode)
        {
            var variableIdAttribute = variableNode.Attributes["id"];
            var variableId = variableIdAttribute.Value;
            var variableSizeAttribute = variableNode.Attributes["size"];
            var variableSize = variableSizeAttribute.Value;
            var variableName = string.Empty;
            var domainExpression = string.Empty;
            for (var i = 0; i < variableNode.ChildNodes.Count; i++)
            {
                var childNode = variableNode.ChildNodes[i];
                switch (childNode.Name)
                {
                    case "name":
                        variableName = childNode.InnerText;
                        break;

                    case "domain":
                        domainExpression = childNode.InnerText;
                        break;
                }
            }

            var variableModel = new AggregateVariableModel(_model.Workspace, new ModelName(variableName), Convert.ToInt32(variableSize), new InlineDomainModel(domainExpression));
            variableModel.Id = Convert.ToInt32(variableId);
            _model.AddVariable(variableModel);
        }
    }
}