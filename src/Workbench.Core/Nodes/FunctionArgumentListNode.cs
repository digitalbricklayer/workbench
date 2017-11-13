using System.Collections.Generic;
using System.Collections.ObjectModel;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class FunctionArgumentListNode : AstNode
    {
        private readonly IList<FunctionCallArgumentNode> arguments;

        public FunctionArgumentListNode()
        {
            this.arguments = new List<FunctionCallArgumentNode>();
        }

        public IReadOnlyCollection<FunctionCallArgumentNode> Arguments
        {
            get
            {
                return new ReadOnlyCollection<FunctionCallArgumentNode>(this.arguments);
            }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            foreach (var node in treeNode.ChildNodes)
            {
                var newArgument = (FunctionCallArgumentNode) AddChild("argument", node);
                this.arguments.Add(newArgument);
            }
        }
    }
}