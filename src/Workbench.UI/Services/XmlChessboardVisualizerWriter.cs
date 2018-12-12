using System;
using System.Diagnostics.Contracts;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    /// <summary>
    /// Write the chessboard visualizer to the XML document.
    /// </summary>
    internal sealed class XmlChessboardVisualizerWriter : XmlDocumentWriter<ChessboardTabModel>
    {
        internal XmlChessboardVisualizerWriter(XmlDocument theDocument, ChessboardTabModel theChessboardTab)
            : base(theDocument, theChessboardTab)
        {
            Contract.Requires<ArgumentNullException>(theDocument != null);
            Contract.Requires<ArgumentNullException>(theChessboardTab != null);
        }

        internal void Write(XmlElement visualizersRoot)
        {
            Contract.Requires<ArgumentNullException>(visualizersRoot != null);

            var visualizerElement = Document.CreateElement("chessboard");
            var idAttribute = Document.CreateAttribute("id");
            idAttribute.Value = Convert.ToString(Subject.Id);
            visualizerElement.Attributes.Append(idAttribute);
            var nameElement = Document.CreateElement("name");
            var encodedNameNode = Document.CreateCDataSection(Subject.Name);
            nameElement.AppendChild(encodedNameNode);
            visualizerElement.AppendChild(nameElement);
            var titleElement = Document.CreateElement("title");
            var encodedTitleNode = Document.CreateCDataSection(Subject.Title.Text);
            titleElement.AppendChild(encodedTitleNode);
            visualizerElement.AppendChild(titleElement);
            visualizersRoot.AppendChild(visualizerElement);
        }
    }
}