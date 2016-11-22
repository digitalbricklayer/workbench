using System.Collections.Generic;
using System.Collections.ObjectModel;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class StatementListNode : AstNode
    {
        private readonly IList<StatementNode> statements;

        public StatementListNode()
        {
            this.statements = new List<StatementNode>();
        }

        public IReadOnlyCollection<StatementNode> Statements
        {
            get
            {
                return new ReadOnlyCollection<StatementNode>(this.statements);
            }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            foreach (var node in treeNode.ChildNodes)
            {
                var newStatement = (StatementNode)AddChild("statement", node);
                this.statements.Add(newStatement);
            }
        }
    }
}