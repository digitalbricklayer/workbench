using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class ValueOffsetNode : AstNode
    {
        public AstNode InnerExpression { get; set; }
        public bool IsLiteral => InnerExpression is NumberLiteralNode;
        public bool IsCounterReference => InnerExpression is CounterReferenceNode;
        public NumberLiteralNode Literal => InnerExpression as NumberLiteralNode;
        public CounterReferenceNode Counter => InnerExpression as CounterReferenceNode;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            InnerExpression = AddChild("inner", treeNode.ChildNodes[0]);
        }
    }
}
