using System;
using System.Diagnostics.Contracts;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    /// <summary>
    /// Write the chessboard visualizer to the XML document.
    /// </summary>
    internal sealed class XmlChessboardVisualizerWriter
    {
        private readonly XmlDocument _document;
        private readonly ChessboardTabModel _chessboardTab;

        internal XmlChessboardVisualizerWriter(XmlDocument theDocument, ChessboardTabModel theChessboardTab)
        {
            Contract.Requires<ArgumentNullException>(theDocument != null);
            Contract.Requires<ArgumentNullException>(theChessboardTab != null);
            _document = theDocument;
            _chessboardTab = theChessboardTab;
        }

        internal void Write(XmlElement visualizersRoot)
        {
            Contract.Requires<ArgumentNullException>(visualizersRoot != null);

            var visualizerElement = _document.CreateElement("chessboard");
            var idAttribute = _document.CreateAttribute("id");
            idAttribute.Value = Convert.ToString(_chessboardTab.Id);
            visualizerElement.Attributes.Append(idAttribute);
            var nameElement = _document.CreateElement("name");
            var encodedNameNode = _document.CreateCDataSection(_chessboardTab.Name);
            nameElement.AppendChild(encodedNameNode);
            visualizerElement.AppendChild(nameElement);
            var titleElement = _document.CreateElement("title");
            var encodedTitleNode = _document.CreateCDataSection(_chessboardTab.Title.Text);
            titleElement.AppendChild(encodedTitleNode);
            visualizerElement.AppendChild(titleElement);
            visualizersRoot.AppendChild(visualizerElement);
        }
    }
}