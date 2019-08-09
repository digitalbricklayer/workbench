using System.Xml;

namespace Workbench.Services
{
    /// <summary>
    /// Base class for the XML document writer.
    /// </summary>
    /// <typeparam name="T">Subject to write to the document.</typeparam>
    internal abstract class XmlDocumentWriter<T>
    {
        /// <summary>
        /// Gets the XML document being written to.
        /// </summary>
        public XmlDocument Document { get; }

        /// <summary>
        /// Gets the subject to write to the XML document.
        /// </summary>
        public T Subject { get; }

        /// <summary>
        /// Initialize the XML document writer with the XML document and the subject to be written to the document.
        /// </summary>
        /// <param name="theDocument">XML document.</param>
        /// <param name="theObject">Subject to write to the document.</param>
        protected XmlDocumentWriter(XmlDocument theDocument, T theObject)
        {
            Document = theDocument;
            Subject = theObject;
        }
    }
}