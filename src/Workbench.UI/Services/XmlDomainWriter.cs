using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal class XmlDomainWriter
    {
        private readonly XmlDocument _document;
        private readonly ModelModel _model;

        internal XmlDomainWriter(XmlDocument theDocument, ModelModel theModel)
        {
            _document = theDocument;
            _model = theModel;
        }

        internal void Write(XmlElement modelRoot)
        {
            var domainsRoot = _document.CreateElement("domains");
            foreach (var aDomain in _model.SharedDomains)
            {
                var domainElement = _document.CreateElement("domain");
                var nameAttribute = _document.CreateAttribute("name");
                nameAttribute.Value = aDomain.Name;
                domainElement.Attributes.Append(nameAttribute);
                var expressionElement = _document.CreateElement("expression");
                var encodedExpressionNode = _document.CreateCDataSection(aDomain.Expression.Text);
                expressionElement.AppendChild(encodedExpressionNode);
                domainElement.AppendChild(expressionElement);
                domainsRoot.AppendChild(domainElement);
            }
            modelRoot.AppendChild(domainsRoot);
        }
    }
}