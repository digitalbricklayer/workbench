using System;
using System.Diagnostics.Contracts;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal sealed class XmlDisplayWriter
    {
        private readonly DisplayModel _display;
        private readonly XmlDocument _document;

        internal XmlDisplayWriter(XmlDocument theDocument, DisplayModel theDisplay)
        {
            Contract.Requires<ArgumentNullException>(theDocument != null);
            Contract.Requires<ArgumentNullException>(theDisplay != null);
            _display = theDisplay;
            _document = theDocument;
        }

        internal void Write(XmlElement workspaceRoot)
        {
            var displayRoot = _document.CreateElement("display");
            new XmlVisualizerBindingWriter(_document, _display).Write(displayRoot);
            new XmlVisualizerWriter(_document, _display).Write(displayRoot);
            workspaceRoot.AppendChild(displayRoot);
        }
    }
}
