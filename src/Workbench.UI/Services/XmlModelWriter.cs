using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    /// <summary>
    /// Class for writing a model to XML.
    /// </summary>
    internal class XmlModelWriter
    {
        private readonly XmlDocument _document;
        private readonly ModelModel _model;

        internal XmlModelWriter(XmlDocument theDocument, ModelModel theModel)
        {
            _document = theDocument;
            _model = theModel;
        }

        internal void Write(XmlElement workspaceRoot)
        {
            var modelRoot = _document.CreateElement("model");
            var nameAttribute = _document.CreateAttribute("name");
            nameAttribute.Value = _model.Name.Text;
            modelRoot.Attributes.Append(nameAttribute);
            new XmlVariableWriter(_document, _model).Write(modelRoot);
            new XmlConstraintWriter(_document, _model).Write(modelRoot);
            new XmlDomainWriter(_document, _model).Write(modelRoot);
            workspaceRoot.AppendChild(modelRoot);
        }
    }
}