using System;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Nodes
{
    public class DomainNameNode : AstNode
    {
        public string Name { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nameWithPrefix = Convert.ToString(treeNode.Token.Value);
            Contract.Assert(nameWithPrefix[0] == '$');
            Name = nameWithPrefix.Substring(1);
        }
    }
}