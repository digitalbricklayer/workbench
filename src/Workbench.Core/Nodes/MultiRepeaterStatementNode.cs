using System.Linq;
using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter.Ast;

namespace Workbench.Core.Nodes
{
    public class MultiRepeaterStatementNode : AstNode
    {
        public ScopeDeclarationListNode ScopeDeclarations { get; private set; }
        public CounterDeclarationListNode CounterDeclarations { get; private set; }
        public StatementNode Statement { get; private set; }
        
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            // The expander statement can be empty and still valid...
            if (!treeNode.ChildNodes.Any()) return;
            CounterDeclarations = (CounterDeclarationListNode) AddChild("counter declarations", treeNode.ChildNodes[0]);
            ScopeDeclarations = (ScopeDeclarationListNode)AddChild("scope declarations", treeNode.ChildNodes[2]);
            if (treeNode.ChildNodes.Count > 3)
            {
                Statement = (StatementNode) AddChild("statement", treeNode.ChildNodes[3]);
            }
        }
    }
}
