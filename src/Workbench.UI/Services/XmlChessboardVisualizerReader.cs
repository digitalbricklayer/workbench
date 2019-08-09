using System;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    /// <summary>
    /// Read the chessboard visualizer from the XML document.
    /// </summary>
    internal sealed class XmlChessboardVisualizerReader
    {
        private DisplayModel _display;

        internal XmlChessboardVisualizerReader(DisplayModel theDisplay)
        {
            _display = theDisplay;
        }

        internal void Read(XmlNode chessboardNode)
        {
            var chessboardIdAttribute = chessboardNode.Attributes["id"];
            var chessboardId = chessboardIdAttribute.Value;
            var name = string.Empty;
            var title = string.Empty;
            for (var i = 0; i < chessboardNode.ChildNodes.Count; i++)
            {
                var childNode = chessboardNode.ChildNodes[i];
                switch (childNode.Name)
                {
                    case "name":
                        name = childNode.InnerText;
                        break;

                    case "title":
                        title = childNode.InnerText;
                        break;
                }
            }

            var newChessboardVisualizer = new ChessboardTabModel(new ChessboardModel(new ModelName(name)),
                                                                 new WorkspaceTabTitle(title));
            newChessboardVisualizer.Id = Convert.ToInt32(chessboardId);
            _display.AddVisualizer(newChessboardVisualizer);
        }
    }
}
