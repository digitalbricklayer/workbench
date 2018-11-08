using System;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal class XmlConstraintReader
    {
        private readonly ModelModel _model;

        internal XmlConstraintReader(ModelModel theModel)
        {
            _model = theModel;
        }

        internal void Read(XmlNodeList constraintNodeList)
        {
            for (var i = 0; i < constraintNodeList.Count; i++)
            {
                var constraintNode = constraintNodeList[i];
                switch (constraintNode.Name)
                {
                    case "expression-constraint":
                        ReadExpressionConstraint(constraintNode);
                        break;

                    case "all-different-constraint":
                        ReadAllDifferentConstraint(constraintNode);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void ReadAllDifferentConstraint(XmlNode constraintNode)
        {
            var constraintNameAttribute = constraintNode.Attributes["name"];
            var constraintName = constraintNameAttribute.Value;
            var expression = string.Empty;
            for (var i = 0; i < constraintNode.ChildNodes.Count; i++)
            {
                var childNode = constraintNode.ChildNodes[i];
                switch (childNode.Name)
                {
                    case "expression":
                        expression = childNode.InnerText;
                        break;
                }
            }

            _model.AddConstraint(new ExpressionConstraintModel(new ModelName(constraintName), new ConstraintExpressionModel(expression)));
        }

        private void ReadExpressionConstraint(XmlNode constraintNode)
        {
            var constraintNameAttribute = constraintNode.Attributes["name"];
            var constraintName = constraintNameAttribute.Value;
            var expression = string.Empty;
            for (var i = 0; i < constraintNode.ChildNodes.Count; i++)
            {
                var childNode = constraintNode.ChildNodes[i];
                switch (childNode.Name)
                {
                    case "expression":
                        expression = childNode.InnerText;
                        break;
                }
            }

            _model.AddConstraint(new ExpressionConstraintModel(new ModelName(constraintName), new ConstraintExpressionModel(expression)));
        }
    }
}