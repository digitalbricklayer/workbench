using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class SharedDomainReferenceNode : AstNode
    {
        public DomainNameNode DomainName { get; set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            DomainName = (DomainNameNode) AddChild("domain name", treeNode.ChildNodes[0]);
        }
    }
}