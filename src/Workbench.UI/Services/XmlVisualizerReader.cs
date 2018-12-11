using System;
using System.Diagnostics.Contracts;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    /// <summary>
    /// Read the visualizers from the XML document.
    /// </summary>
    internal sealed class XmlVisualizerReader
    {
        private readonly DisplayModel _display;

        internal XmlVisualizerReader(DisplayModel theDisplay)
        {
            Contract.Requires<ArgumentNullException>(theDisplay != null);
            _display = theDisplay;
        }

        internal void Read(XmlNodeList visualizerNodeList)
        {
            Contract.Requires<ArgumentNullException>(visualizerNodeList != null);

            for (var i = 0; i < visualizerNodeList.Count; i++)
            {
                var visualizerNode = visualizerNodeList[i];
                switch (visualizerNode.Name)
                {
                    case "chessboard":
                        new XmlChessboardVisualizerReader(_display).Read(visualizerNode);
                        break;

                    case "table":
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}
