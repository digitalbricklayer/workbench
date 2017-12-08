using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class AggregateVariableReferenceNode : AstNode
    {
        /// <summary>
        /// Gets the aggregate variable name.
        /// </summary>
        public string VariableName { get; private set; }

        /// <summary>
        /// Gets the subscript statement.
        /// </summary>
        public SubscriptStatementNode SubscriptStatement { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var variableNameWithPrefix = treeNode.ChildNodes[0].FindTokenAndGetText();
            VariableName = variableNameWithPrefix.Substring(1);
            // Child nodes 1 and 3 contain the opening and closing brackets
            SubscriptStatement = (SubscriptStatementNode) AddChild("Subscript Statement", treeNode.ChildNodes[2]);
        }
    }
}
