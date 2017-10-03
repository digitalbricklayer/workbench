using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class VariableDomainExpressionNode : AstNode
    {
        public AstNode Inner { get; private set; }

        /// <summary>
        /// Gets the domain reference expression.
        /// </summary>
        public SharedDomainReferenceNode DomainReference
        {
            get
            {
                if (Inner is SharedDomainReferenceNode)
                {
                    var sharedDomainReference = (SharedDomainReferenceNode) Inner;
                    return sharedDomainReference;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the inline domain expression.
        /// </summary>
        public DomainExpressionNode InlineDomain
        {
            get
            {
                if (Inner is DomainExpressionNode)
                {
                    var inlineDomain = (DomainExpressionNode) Inner;
                    return inlineDomain;
                }

                return null;
            }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Inner = AddChild("expression", treeNode.ChildNodes[0]);
        }
    }
}