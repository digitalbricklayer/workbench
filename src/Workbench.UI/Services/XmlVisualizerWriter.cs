using System;
using System.Diagnostics.Contracts;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    /// <summary>
    /// Write the visualizers to the XML document.
    /// </summary>
    internal sealed class XmlVisualizerWriter
    {
        private readonly XmlDocument _document;
        private readonly DisplayModel _display;

        internal XmlVisualizerWriter(XmlDocument theDocument, DisplayModel theDisplay)
        {
            Contract.Requires<ArgumentNullException>(theDocument != null);
            Contract.Requires<ArgumentNullException>(theDisplay != null);
            _display = theDisplay;
            _document = theDocument;
        }

        internal void Write(XmlElement displayRoot)
        {
            var visualizersRoot = _document.CreateElement("visualizers");
            foreach (var aVisualizer in _display.Visualizers)
            {
                switch (aVisualizer)
                {
                    case ChessboardTabModel aChessboardTab:
                        new XmlChessboardVisualizerWriter(_document, aChessboardTab).Write(visualizersRoot);
                        break;

                    case TableTabModel _:
                    default:
                        throw new NotImplementedException();
                }
            }
            displayRoot.AppendChild(visualizersRoot);
        }
    }
}
