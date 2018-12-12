using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal sealed class XmlDisplayWriter : XmlDocumentWriter<DisplayModel>
    {
        internal XmlDisplayWriter(XmlDocument theDocument, DisplayModel theDisplay)
            : base(theDocument, theDisplay)
        {
        }

        internal void Write(XmlElement workspaceRoot)
        {
            var displayRoot = Document.CreateElement("display");
            new XmlVisualizerBindingWriter(Document, Subject.Bindings).Write(displayRoot);
            new XmlVisualizerWriter(Document, Subject.Visualizers).Write(displayRoot);
            workspaceRoot.AppendChild(displayRoot);
        }
    }
}
