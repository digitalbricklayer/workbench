using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class VisualizerExpressionNode : AstNode
    {
        /// <summary>
        /// Gets the inner repeater statement.
        /// </summary>
        public MultiRepeaterStatementNode InnerExpression { get; private set; }

        /// <summary>
        /// Gets whether the expression is empty.
        /// </summary>
        public bool IsEmpty => InnerExpression == null;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            // A visualizer expression can be empty and still be perfectly valid...
            if (!treeNode.ChildNodes.Any()) return;
            InnerExpression = (MultiRepeaterStatementNode) AddChild("expression", treeNode.ChildNodes[0]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            return base.DoEvaluate(thread);
        }
    }
}
