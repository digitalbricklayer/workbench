using System;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    /// <summary>
    /// Class for writing a workspace model to XML.
    /// </summary>
    public class XmlWorkspaceWriter : IWorkspaceWriter
    {
        public void Write(string filename, WorkspaceModel theWorkspace)
        {
            var workspaceDocument = new XmlDocument();
            var workspaceRoot = workspaceDocument.CreateElement("workspace");
            workspaceDocument.AppendChild(workspaceRoot);
            new XmlModelWriter(workspaceDocument, theWorkspace.Model).Write(workspaceRoot);
            workspaceDocument.Save(filename);
        }
    }

    /// <summary>
    /// Class for writing a model to XML.
    /// </summary>
    internal class XmlModelWriter
    {
        private readonly XmlDocument _document;
        private readonly ModelModel _model;

        internal XmlModelWriter(XmlDocument theDocument, ModelModel theModel)
        {
            _document = theDocument;
            _model = theModel;
        }

        internal void Write(XmlElement workspaceRoot)
        {
            var modelRoot = _document.CreateElement("model");
            var nameAttribute = _document.CreateAttribute("name");
            nameAttribute.Value = _model.Name.Text;
            modelRoot.Attributes.Append(nameAttribute);
            new XmlVariableWriter(_document, _model).Write(modelRoot);
            new XmlConstraintWriter(_document, _model).Write(modelRoot);
            new XmlDomainWriter(_document, _model).Write(modelRoot);
            workspaceRoot.AppendChild(modelRoot);
        }
    }

    internal class XmlDomainWriter
    {
        private readonly XmlDocument _document;
        private readonly ModelModel _model;

        internal XmlDomainWriter(XmlDocument theDocument, ModelModel theModel)
        {
            _document = theDocument;
            _model = theModel;
        }

        internal void Write(XmlElement modelRoot)
        {
            var domainsRoot = _document.CreateElement("domains");
            foreach (var aDomain in _model.Domains)
            {
                var domainElement = _document.CreateElement("domain");
                var nameAttribute = _document.CreateAttribute("name");
                nameAttribute.Value = aDomain.Name;
                domainElement.Attributes.Append(nameAttribute);
                var expressionElement = _document.CreateElement("expression");
                var encodedExpressionNode = _document.CreateCDataSection(aDomain.Expression.Text);
                expressionElement.AppendChild(encodedExpressionNode);
                domainElement.AppendChild(expressionElement);
                domainsRoot.AppendChild(domainElement);
            }
            modelRoot.AppendChild(domainsRoot);
        }
    }

    internal class XmlVariableWriter
    {
        private readonly XmlDocument _document;
        private readonly ModelModel _model;

        public XmlVariableWriter(XmlDocument theDocument, ModelModel theModel)
        {
            _document = theDocument;
            _model = theModel;
        }

        public void Write(XmlElement modelRoot)
        {
            var variablesRoot = _document.CreateElement("variables");
            foreach (var aVariable in _model.Variables)
            {
                switch (aVariable)
                {
                    case SingletonVariableModel singletonVariable:
                        WriteVariable(singletonVariable, variablesRoot);
                        break;

                    case AggregateVariableModel aggregateVariable:
                        WriteVariable(aggregateVariable, variablesRoot);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
            modelRoot.AppendChild(variablesRoot);
        }

        private void WriteVariable(SingletonVariableModel aVariable, XmlElement variablesRoot)
        {
            var variableElement = _document.CreateElement("singleton-variable");
            var nameAttribute = _document.CreateAttribute("name");
            nameAttribute.Value = aVariable.Name;
            variableElement.Attributes.Append(nameAttribute);
            var domainElement = _document.CreateElement("domain");
            var encodedDomainNode = _document.CreateCDataSection(aVariable.DomainExpression.Text);
            domainElement.AppendChild(encodedDomainNode);
            variableElement.AppendChild(domainElement);
            variablesRoot.AppendChild(variableElement);
        }

        private void WriteVariable(AggregateVariableModel aVariable, XmlElement variablesRoot)
        {
            var variableElement = _document.CreateElement("aggregate-variable");
            var nameAttribute = _document.CreateAttribute("name");
            nameAttribute.Value = aVariable.Name;
            variableElement.Attributes.Append(nameAttribute);
            var sizeAttribute = _document.CreateAttribute("size");
            sizeAttribute.Value = Convert.ToString(aVariable.GetSize());
            variableElement.Attributes.Append(sizeAttribute);
            var domainElement = _document.CreateElement("domain");
            var encodedDomainNode = _document.CreateCDataSection(aVariable.DomainExpression.Text);
            domainElement.AppendChild(encodedDomainNode);
            variableElement.AppendChild(domainElement);
            variablesRoot.AppendChild(variableElement);
        }
    }

    internal class XmlConstraintWriter
    {
        private readonly XmlDocument _document;
        private readonly ModelModel _model;

        internal XmlConstraintWriter(XmlDocument theDocument, ModelModel theModel)
        {
            _document = theDocument;
            _model = theModel;
        }

        internal void Write(XmlElement modelRoot)
        {
            var constraintsRoot = _document.CreateElement("constraints");
            foreach (var aConstraint in _model.Constraints)
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
            var expressionElement = _document.CreateElement("expression-constraint");
            var nameAttribute = _document.CreateAttribute("name");
            nameAttribute.Value = expressionConstraint.Name;
            expressionElement.Attributes.Append(nameAttribute);
            var xElement = _document.CreateElement("expression");
            var encodedExpressionNode = _document.CreateCDataSection(expressionConstraint.Expression.Text);
            xElement.AppendChild(encodedExpressionNode);
            expressionElement.AppendChild(xElement);
            constraintsRoot.AppendChild(expressionElement);
        }

        private void WriteConstraint(AllDifferentConstraintModel allDifferentConstraint, XmlElement constraintsRoot)
        {
            var allDifferentElement = _document.CreateElement("all-different-constraint");
            var nameAttribute = _document.CreateAttribute("name");
            nameAttribute.Value = allDifferentConstraint.Name;
            allDifferentElement.Attributes.Append(nameAttribute);
            var expressionElement = _document.CreateElement("expression");
            var encodedExpressionNode = _document.CreateCDataSection(allDifferentConstraint.Expression.Text);
            expressionElement.AppendChild(encodedExpressionNode);
            allDifferentElement.AppendChild(expressionElement);
            constraintsRoot.AppendChild(allDifferentElement);
        }
    }
}
