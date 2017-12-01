using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class SingletonVariableReferenceNode : AstNode
    {
        public string VariableName { get; private set; }

#if false
        public override void Accept(IConstraintExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
#endif

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var variableNameWithPrefix = treeNode.ChildNodes[0].FindTokenAndGetText();
            VariableName = variableNameWithPrefix.Substring(1);
        }
    }
}