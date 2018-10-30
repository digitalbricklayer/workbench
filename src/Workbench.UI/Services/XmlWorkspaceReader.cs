using System;
using System.Diagnostics.Contracts;
using System.Xml;
using Workbench.Core.Models;

namespace Workbench.Services
{
    public class XmlWorkspaceReader : IWorkspaceReader
    {
        public WorkspaceModel Read(string filename)
        {
            var workspaceDocument = new XmlDocument();
            workspaceDocument.Load(filename);
            var workspaceNode = workspaceDocument.FirstChild;
            if (!workspaceNode.HasChildNodes) throw new Exception("Invalid workspace file.");
            var theWorkspace = new WorkspaceModel();
            for (var i = 0; i < workspaceNode.ChildNodes.Count; i++)
            {
                var topLevelNode = workspaceNode.ChildNodes[i];
                switch (topLevelNode.Name.ToLower())
                {
                    case "model":
                        theWorkspace.Model = new XmlModelReader(theWorkspace).Read(topLevelNode);
                        break;

                    default:
                        throw new NotImplementedException("Unknown top level node.");
                }
            }

            return theWorkspace;
        }
    }

    internal class XmlModelReader
    {
        private readonly WorkspaceModel _workspace;

        public XmlModelReader(WorkspaceModel theWorkspace)
        {
            _workspace = theWorkspace;
        }

        internal ModelModel Read(XmlNode theModelNode)
        {
            var modelNameAttribute = theModelNode.Attributes["name"];
            var modelName = modelNameAttribute.Value;
            _workspace.Model.Name = new ModelName(modelName);
            var model = _workspace.Model;
            for (var i = 0; i < theModelNode.ChildNodes.Count; i++)
            {
                var childNode = theModelNode.ChildNodes[i];
                switch (childNode.Name.ToLower())
                {
                    case "constraints":
                        new XmlConstraintReader(model).Read(childNode.ChildNodes);
                        break;

                    case "domains":
                        new XmlDomainReader(model).Read(childNode.ChildNodes);
                        break;

                    case "variables":
                        new XmlVariableReader(_workspace).Read(childNode.ChildNodes);
                        break;

                    default:
                        throw new NotImplementedException("Unknown model node.");
                }
            }

            return model;
        }
    }

    internal class XmlDomainReader
    {
        private readonly ModelModel _model;

        internal XmlDomainReader(ModelModel theModel)
        {
            _model = theModel;
        }

        internal void Read(XmlNodeList domainNodeList)
        {
            for (var i = 0; i < domainNodeList.Count; i++)
            {
                var domainNode = domainNodeList[i];
                switch (domainNode.Name)
                {
                    case "domain":
                        var domainNameAttribute = domainNode.Attributes["name"];
                        var domainName = domainNameAttribute.Value;
                        var expression = string.Empty;
                        for (var z = 0; z < domainNode.ChildNodes.Count; z++)
                        {
                            var childNode = domainNode.ChildNodes[z];
                            switch (childNode.Name)
                            {
                                case "expression":
                                    expression = childNode.InnerText;
                                    break;
                            }
                        }

                        _model.AddSharedDomain(new SharedDomainModel(new ModelName(domainName), new SharedDomainExpressionModel(expression)));
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }

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

    internal class XmlVariableReader
    {
        private readonly ModelModel _model;
        private readonly WorkspaceModel _workspace;

        internal XmlVariableReader(WorkspaceModel theWorkspace)
        {
            Contract.Requires<ArgumentNullException>(theWorkspace != null);
            _workspace = theWorkspace;
            _model = _workspace.Model;
        }

        internal void Read(XmlNodeList variableNodeList)
        {
            for (var i = 0; i < variableNodeList.Count; i++)
            {
                var constraintNode = variableNodeList[i];
                switch (constraintNode.Name)
                {
                    case "aggregate-variable":
                        ReadAggregateVariable(constraintNode);
                        break;

                    case "singleton-variable":
                        ReadSingletonVariable(constraintNode);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void ReadSingletonVariable(XmlNode constraintNode)
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

            _model.AddVariable(new SingletonVariableModel(_model, new ModelName(constraintName), new InlineDomainModel(expression)));
        }

        private void ReadAggregateVariable(XmlNode constraintNode)
        {
            var constraintNameAttribute = constraintNode.Attributes["name"];
            var constraintName = constraintNameAttribute.Value;
            var constraintSizeAttribute = constraintNode.Attributes["size"];
            var constraintSize = constraintSizeAttribute.Value;
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

            _model.AddVariable(new AggregateVariableModel(_model.Workspace, new ModelName(constraintName), Convert.ToInt32(constraintSize), new InlineDomainModel(expression)));
        }
    }
}
