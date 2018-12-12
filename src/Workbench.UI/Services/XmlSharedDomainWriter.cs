using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal class XmlSharedDomainWriter : XmlDocumentWriter<IList<SharedDomainModel>>
    {
        internal XmlSharedDomainWriter(XmlDocument theDocument, ObservableCollection<SharedDomainModel> theSharedDomains)
            : base(theDocument, theSharedDomains)
        {
        }

        internal void Write(XmlElement modelRoot)
        {
            var domainsRoot = Document.CreateElement("domains");
            foreach (var aDomain in Subject)
            {
                var domainElement = Document.CreateElement("domain");
                var idAttribute = Document.CreateAttribute("id");
                idAttribute.Value = Convert.ToString(aDomain.Id);
                domainElement.Attributes.Append(idAttribute);
                var nameElement = Document.CreateElement("name");
                var encodedNameNode = Document.CreateCDataSection(aDomain.Name);
                nameElement.AppendChild(encodedNameNode);
                domainElement.AppendChild(nameElement);
                var expressionElement = Document.CreateElement("expression");
                var encodedExpressionNode = Document.CreateCDataSection(aDomain.Expression.Text);
                expressionElement.AppendChild(encodedExpressionNode);
                domainElement.AppendChild(expressionElement);
                domainsRoot.AppendChild(domainElement);
            }
            modelRoot.AppendChild(domainsRoot);
        }
    }
}