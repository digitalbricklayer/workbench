using System;
using System.Collections.Generic;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal sealed class XmlVisualizerBindingWriter : XmlDocumentWriter<IReadOnlyCollection<VisualizerBindingExpressionModel>>
    {
        internal XmlVisualizerBindingWriter(XmlDocument theDocument, IReadOnlyCollection<VisualizerBindingExpressionModel> theVisualizerBindings)
            : base(theDocument, theVisualizerBindings)
        {
        }

        internal void Write(XmlElement displayRoot)
        {
            var bindingsRoot = Document.CreateElement("bindings");
            WriteVisualizerBindings(bindingsRoot);
            displayRoot.AppendChild(bindingsRoot);
        }

        private void WriteVisualizerBindings(XmlElement bindingsRoot)
        {
            foreach (var aBinding in Subject)
            {
                WriteVisualizerBinding(aBinding, bindingsRoot);
            }
        }

        private void WriteVisualizerBinding(VisualizerBindingExpressionModel theBinding, XmlElement bindingsRoot)
        {
            var bindingElement = Document.CreateElement("binding");
            var idAttribute = Document.CreateAttribute("id");
            idAttribute.Value = Convert.ToString(theBinding.Id);
            bindingElement.Attributes.Append(idAttribute);
            var expressionElement = Document.CreateElement("expression");
            var encodedExpressionNode = Document.CreateCDataSection(theBinding.Text);
            expressionElement.AppendChild(encodedExpressionNode);
            bindingElement.AppendChild(expressionElement);
            bindingsRoot.AppendChild(bindingElement);
        }
    }
}