using System;
using System.Diagnostics.Contracts;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal sealed class XmlVisualizerBindingWriter
    {
        private readonly XmlDocument _document;
        private readonly DisplayModel _display;

        internal XmlVisualizerBindingWriter(XmlDocument theDocument, DisplayModel theDisplay)
        {
            Contract.Requires<ArgumentNullException>(theDocument != null);
            Contract.Requires<ArgumentNullException>(theDisplay != null);
            _display = theDisplay;
            _document = theDocument;
        }

        internal void Write(XmlElement displayRoot)
        {
            var bindingsRoot = _document.CreateElement("bindings");
            WriteVisualizerBindings(bindingsRoot);
            displayRoot.AppendChild(bindingsRoot);
        }

        private void WriteVisualizerBindings(XmlElement bindingsRoot)
        {
            foreach (var aBinding in _display.Bindings)
            {
                WriteVisualizerBinding(aBinding, bindingsRoot);
            }
        }

        private void WriteVisualizerBinding(VisualizerBindingExpressionModel theBinding, XmlElement bindingsRoot)
        {
            var bindingElement = _document.CreateElement("binding");
            var idAttribute = _document.CreateAttribute("id");
            idAttribute.Value = Convert.ToString(theBinding.Id);
            bindingElement.Attributes.Append(idAttribute);
            var expressionElement = _document.CreateElement("expression");
            var encodedExpressionNode = _document.CreateCDataSection(theBinding.Text);
            expressionElement.AppendChild(encodedExpressionNode);
            bindingElement.AppendChild(expressionElement);
            bindingsRoot.AppendChild(bindingElement);
        }
    }
}