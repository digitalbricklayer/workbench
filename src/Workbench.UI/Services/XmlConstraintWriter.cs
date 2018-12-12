using System;
using System.Collections.Generic;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    internal class XmlConstraintWriter : XmlDocumentWriter<IList<ConstraintModel>>
    {
        internal XmlConstraintWriter(XmlDocument theDocument, IList<ConstraintModel> theConstraints)
            : base(theDocument, theConstraints)
        {
        }

        internal void Write(XmlElement modelRoot)
        {
            var constraintsRoot = Document.CreateElement("constraints");
            foreach (var aConstraint in Subject)
            {
                switch (aConstraint)
                {
                    case AllDifferentConstraintModel allDifferentConstraint:
                        WriteConstraint(allDifferentConstraint, constraintsRoot);
                        break;

                    case ExpressionConstraintModel expressionConstraint:
                        WriteConstraint(expressionConstraint, constraintsRoot);
                        break;

                    default:
                        throw new NotImplementedException("Unknown constraint type.");
                }
            }
            modelRoot.AppendChild(constraintsRoot);
        }

        private void WriteConstraint(ExpressionConstraintModel expressionConstraint, XmlElement constraintsRoot)
        {
            var expressionConstraintElement = Document.CreateElement("expression-constraint");
            var idAttribute = Document.CreateAttribute("id");
            idAttribute.Value = Convert.ToString(expressionConstraint.Id);
            expressionConstraintElement.Attributes.Append(idAttribute);
            var nameElement = Document.CreateElement("name");
            var encodedNameNode = Document.CreateCDataSection(expressionConstraint.Name);
            nameElement.AppendChild(encodedNameNode);
            expressionConstraintElement.AppendChild(nameElement);
            var expressionElement = Document.CreateElement("expression");
            var encodedExpressionNode = Document.CreateCDataSection(expressionConstraint.Expression.Text);
            expressionElement.AppendChild(encodedExpressionNode);
            expressionConstraintElement.AppendChild(expressionElement);
            constraintsRoot.AppendChild(expressionConstraintElement);
        }

        private void WriteConstraint(AllDifferentConstraintModel allDifferentConstraint, XmlElement constraintsRoot)
        {
            var allDifferentElement = Document.CreateElement("all-different-constraint");
            var idAttribute = Document.CreateAttribute("id");
            idAttribute.Value = Convert.ToString(allDifferentConstraint.Id);
            allDifferentElement.Attributes.Append(idAttribute);
            var nameElement = Document.CreateElement("name");
            var encodedNameNode = Document.CreateCDataSection(allDifferentConstraint.Name);
            nameElement.AppendChild(encodedNameNode);
            allDifferentElement.AppendChild(nameElement);
            var expressionElement = Document.CreateElement("expression");
            var encodedExpressionNode = Document.CreateCDataSection(allDifferentConstraint.Expression.Text);
            expressionElement.AppendChild(encodedExpressionNode);
            allDifferentElement.AppendChild(expressionElement);
            constraintsRoot.AppendChild(allDifferentElement);
        }
    }
}