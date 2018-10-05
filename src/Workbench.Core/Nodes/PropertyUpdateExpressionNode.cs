using System.Linq;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class PropertyUpdateExpressionNode : AstNode
    {
        /// <summary>
        /// Gets the inner statement.
        /// </summary>
        public AstNode InnerExpression { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            // A property update expression can be empty and still be perfectly valid...
            if (!treeNode.ChildNodes.Any()) return;
            InnerExpression = AddChild("inner", treeNode.ChildNodes[0]);
        }
    }
}
