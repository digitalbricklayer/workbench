using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class ValueReferenceStatementNode : AstNode
    {
        public VariableNameNode VariableName { get; private set; }
        public ValueOffsetNode ValueOffset { get; private set; }
        public bool IsAggregateValue => ValueOffset != null;
        public bool IsSingletonValue => ValueOffset == null;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            VariableName = (VariableNameNode) AddChild("inner", treeNode.ChildNodes[0]);
            // Check to see if this is an aggregate value reference
            if (treeNode.ChildNodes.Count > 1)
                ValueOffset = (ValueOffsetNode) AddChild("offset", treeNode.ChildNodes[1]);
        }
    }
}
