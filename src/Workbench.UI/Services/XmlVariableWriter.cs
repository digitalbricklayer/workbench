using System;
using System.Collections.Generic;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal class XmlVariableWriter : XmlDocumentWriter<IList<VariableModel>>
    {
        internal XmlVariableWriter(XmlDocument theDocument, IList<VariableModel> theVariables)
            : base(theDocument, theVariables)
        {
        }

        internal void Write(XmlElement modelRoot)
        {
            var variablesRoot = Document.CreateElement("variables");
            foreach (var aVariable in Subject)
            {
                switch (aVariable)
                {
                    case SingletonVariableModel singletonVariable:
                        WriteVariable(singletonVariable, variablesRoot);
                        break;

                    case AggregateVariableModel aggregateVariable:
                        WriteVariable(aggregateVariable, variablesRoot);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
            modelRoot.AppendChild(variablesRoot);
        }

        private void WriteVariable(SingletonVariableModel aVariable, XmlElement variablesRoot)
        {
            var variableElement = Document.CreateElement("singleton-variable");
            var idAttribute = Document.CreateAttribute("id");
            idAttribute.Value = Convert.ToString(aVariable.Id);
            variableElement.Attributes.Append(idAttribute);
            var nameElement = Document.CreateElement("name");
            var encodedNameNode = Document.CreateCDataSection(aVariable.Name);
            nameElement.AppendChild(encodedNameNode);
            variableElement.AppendChild(nameElement);
            var domainElement = Document.CreateElement("domain");
            var encodedDomainNode = Document.CreateCDataSection(aVariable.DomainExpression.Text);
            domainElement.AppendChild(encodedDomainNode);
            variableElement.AppendChild(domainElement);
            variablesRoot.AppendChild(variableElement);
        }

        private void WriteVariable(AggregateVariableModel aVariable, XmlElement variablesRoot)
        {
            var variableElement = Document.CreateElement("aggregate-variable");
            var idAttribute = Document.CreateAttribute("id");
            idAttribute.Value = Convert.ToString(aVariable.Id);
            variableElement.Attributes.Append(idAttribute);
            var nameElement = Document.CreateElement("name");
            var encodedNameNode = Document.CreateCDataSection(aVariable.Name);
            nameElement.AppendChild(encodedNameNode);
            variableElement.AppendChild(nameElement);
            var sizeAttribute = Document.CreateAttribute("size");
            sizeAttribute.Value = Convert.ToString(aVariable.GetSize());
            variableElement.Attributes.Append(sizeAttribute);
            var domainElement = Document.CreateElement("domain");
            var encodedDomainNode = Document.CreateCDataSection(aVariable.DomainExpression.Text);
            domainElement.AppendChild(encodedDomainNode);
            variableElement.AppendChild(domainElement);
            variablesRoot.AppendChild(variableElement);
        }
    }
}