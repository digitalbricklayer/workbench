using System;
using Irony.Interpreter.Ast;

namespace Workbench.Core.Nodes
{
    /// <summary>
    /// Base for all constraint expression nodes.
    /// </summary>
    public abstract class ConstraintExpressionBaseNode : AstNode
    {
        /// <summary>
        /// Accept a visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public abstract void Accept(IConstraintExpressionVisitor visitor);
    }
}
