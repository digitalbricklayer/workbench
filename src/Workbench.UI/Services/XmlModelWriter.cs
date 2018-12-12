using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    /// <summary>
    /// Class for writing a model to XML.
    /// </summary>
    internal class XmlModelWriter : XmlDocumentWriter<ModelModel>
    {
        internal XmlModelWriter(XmlDocument theDocument, ModelModel theModel)
            : base(theDocument, theModel)
        {
        }

        internal void Write(XmlElement workspaceRoot)
        {
            var modelRoot = Document.CreateElement("model");
            var nameElement = Document.CreateElement("name");
            var encodedNameNode = Document.CreateCDataSection(Subject.Name);
            nameElement.AppendChild(encodedNameNode);
            modelRoot.AppendChild(nameElement);
            new XmlVariableWriter(Document, Subject.Variables).Write(modelRoot);
            new XmlConstraintWriter(Document, Subject.Constraints).Write(modelRoot);
            new XmlSharedDomainWriter(Document, Subject.SharedDomains).Write(modelRoot);
            workspaceRoot.AppendChild(modelRoot);
        }
    }
}