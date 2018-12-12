using System;
using System.Collections.Generic;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    /// <summary>
    /// Write the visualizers to the XML document.
    /// </summary>
    internal sealed class XmlVisualizerWriter : XmlDocumentWriter<IList<VisualizerModel>>
    {
        internal XmlVisualizerWriter(XmlDocument theDocument, IList<VisualizerModel> theVisualizers)
            : base(theDocument, theVisualizers)
        {
        }

        internal void Write(XmlElement displayRoot)
        {
            var visualizersRoot = Document.CreateElement("visualizers");
            foreach (var aVisualizer in Subject)
            {
                switch (aVisualizer)
                {
                    case ChessboardTabModel aChessboardTab:
                        new XmlChessboardVisualizerWriter(Document, aChessboardTab).Write(visualizersRoot);
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
