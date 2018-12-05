using System;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal class XmlVariableWriter
    {
        private readonly XmlDocument _document;
        private readonly ModelModel _model;

        public XmlVariableWriter(XmlDocument theDocument, ModelModel theModel)
        {
            _document = theDocument;
            _model = theModel;
        }

        public void Write(XmlElement modelRoot)
        {
            var variablesRoot = _document.CreateElement("variables");
            foreach (var aVariable in _model.Variables)
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
            var variableElement = _document.CreateElement("singleton-variable");
            var idAttribute = _document.CreateAttribute("id");
            idAttribute.Value = Convert.ToString(aVariable.Id);
            variableElement.Attributes.Append(idAttribute);
            var nameAttribute = _document.CreateAttribute("name");
            nameAttribute.Value = aVariable.Name;
            variableElement.Attributes.Append(nameAttribute);
            var domainElement = _document.CreateElement("domain");
            var encodedDomainNode = _document.CreateCDataSection(aVariable.DomainExpression.Text);
            domainElement.AppendChild(encodedDomainNode);
            variableElement.AppendChild(domainElement);
            variablesRoot.AppendChild(variableElement);
        }

        private void WriteVariable(AggregateVariableModel aVariable, XmlElement variablesRoot)
        {
            var variableElement = _document.CreateElement("aggregate-variable");
            var idAttribute = _document.CreateAttribute("id");
            idAttribute.Value = Convert.ToString(aVariable.Id);
            variableElement.Attributes.Append(idAttribute);
            var nameAttribute = _document.CreateAttribute("name");
            nameAttribute.Value = aVariable.Name;
            variableElement.Attributes.Append(nameAttribute);
            var sizeAttribute = _document.CreateAttribute("size");
            sizeAttribute.Value = Convert.ToString(aVariable.GetSize());
            variableElement.Attributes.Append(sizeAttribute);
            var domainElement = _document.CreateElement("domain");
            var encodedDomainNode = _document.CreateCDataSection(aVariable.DomainExpression.Text);
            domainElement.AppendChild(encodedDomainNode);
            variableElement.AppendChild(domainElement);
            variablesRoot.AppendChild(variableElement);
        }
    }
}