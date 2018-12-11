using System;
using System.Diagnostics.Contracts;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal sealed class XmlVisualizerBindingReader
    {
        private readonly DisplayModel _display;

        internal XmlVisualizerBindingReader(DisplayModel theDisplay)
        {
            Contract.Requires<ArgumentNullException>(theDisplay != null);
            _display = theDisplay;
        }

        internal void Read(XmlNodeList bindingNodeList)
        {
            Contract.Requires<ArgumentNullException>(bindingNodeList != null);

            for (var i = 0; i < bindingNodeList.Count; i++)
            {
                var bindingNode = bindingNodeList[i];
                switch (bindingNode.Name)
                {
                    case "binding":
                        ReadBindingExpression(bindingNode);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void ReadBindingExpression(XmlNode bindingNode)
        {
            Contract.Requires<ArgumentNullException>(bindingNode != null);

            var constraintIdAttribute = bindingNode.Attributes["id"];
            var constraintId = constraintIdAttribute.Value;
            var expression = string.Empty;
            for (var i = 0; i < bindingNode.ChildNodes.Count; i++)
            {
                var childNode = bindingNode.ChildNodes[i];
                switch (childNode.Name)
                {
                    case "expression":
                        expression = childNode.InnerText;
                        break;
                }
            }

            var newBinding = new VisualizerBindingExpressionModel(expression);
            newBinding.Id = Convert.ToInt32(constraintId);
            _display.AddBindingExpression(newBinding);
        }
    }
}
