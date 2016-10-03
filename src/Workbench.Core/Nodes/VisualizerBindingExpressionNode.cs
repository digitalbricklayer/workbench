using System.Linq;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class VisualizerBindingExpressionNode : AstNode
    {
        /// <summary>
        /// Gets the inner repeater statement.
        /// </summary>
        public AstNode InnerExpression { get; private set; }

        /// <summary>
        /// Gets whether the expression is empty.
        /// </summary>
        public bool IsEmpty => InnerExpression == null;

        /// <summary>
        /// Gets whether the binding has an expander.
        /// </summary>
        public bool HasExpander => Expander != null;

        /// <summary>
        /// Gets the expander statement node.
        /// </summary>
        public MultiRepeaterStatementNode Expander => InnerExpression as MultiRepeaterStatementNode;

        public bool IsIfStatement => InnerExpression is IfStatementNode;
        public bool IsBindingStatement => InnerExpression is CallStatementNode;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            // A visualizer expression can be empty and still be perfectly valid...
            if (!treeNode.ChildNodes.Any()) return;
            InnerExpression = AddChild("inner", treeNode.ChildNodes[0]);
        }
    }
}
