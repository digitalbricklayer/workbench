using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class BucketVariableReferenceNode : AstNode
    {
        /// <summary>
        /// Gets the bucket variable name.
        /// </summary>
        public string BucketName { get; private set; }

        /// <summary>
        /// Gets the subscript statement.
        /// </summary>
        public SubscriptStatementNode SubscriptStatement { get; private set; }

        /// <summary>
        /// Gets the variable name.
        /// </summary>
        public string VariableName { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var bucketNameWithPrefix = treeNode.ChildNodes[0].FindTokenAndGetText();
            BucketName = bucketNameWithPrefix.Substring(1);
            // Child nodes 1 and 3 contain the opening and closing brackets
            SubscriptStatement = (SubscriptStatementNode)AddChild("Subscript Statement", treeNode.ChildNodes[2]);
            // Child node 4 is the dot notation
            VariableName = treeNode.ChildNodes[5].FindTokenAndGetText();
        }
    }
}