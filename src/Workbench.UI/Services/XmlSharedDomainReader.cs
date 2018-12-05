using System;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal class XmlSharedDomainReader
    {
        private readonly ModelModel _model;

        internal XmlSharedDomainReader(ModelModel theModel)
        {
            _model = theModel;
        }

        internal void Read(XmlNodeList domainNodeList)
        {
            for (var i = 0; i < domainNodeList.Count; i++)
            {
                var domainNode = domainNodeList[i];
                switch (domainNode.Name)
                {
                    case "domain":
                        var domainIdAttribute = domainNode.Attributes["id"];
                        var domainId = domainIdAttribute.Value;
                        var domainNameAttribute = domainNode.Attributes["name"];
                        var domainName = domainNameAttribute.Value;
                        var expression = string.Empty;
                        for (var z = 0; z < domainNode.ChildNodes.Count; z++)
                        {
                            var childNode = domainNode.ChildNodes[z];
                            switch (childNode.Name)
                            {
                                case "expression":
                                    expression = childNode.InnerText;
                                    break;
                            }
                        }

                        var domainModel = new SharedDomainModel(_model, new ModelName(domainName), new SharedDomainExpressionModel(expression));
                        domainModel.Id = Convert.ToInt32(domainId);
                        _model.AddSharedDomain(domainModel);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}