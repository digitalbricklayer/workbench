using System.Collections.Generic;
using System.Collections.ObjectModel;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class CallArgumentNodeList : AstNode
    {
        private readonly IList<CallArgumentNode> callArguments;

        public CallArgumentNodeList()
        {
            this.callArguments = new List<CallArgumentNode>();
        }

        public IReadOnlyCollection<CallArgumentNode> Arguments
        {
            get
            {
                return new ReadOnlyCollection<CallArgumentNode>(this.callArguments);
            }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            foreach (var node in treeNode.ChildNodes)
            {
                var newArgument = (CallArgumentNode) AddChild("arguments", node);
                this.callArguments.Add(newArgument);
            }
        }
    }
}
