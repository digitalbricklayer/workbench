using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Irony.Parsing;
using Workbench.Core.Nodes;

namespace Workbench.Core.Parsers
{
    public class ConstraintExpressionParseResult
    {
        public ConstraintExpressionParseResult(ConstraintExpressionParseStatus theParseStatus, ParseTree theParseTree)
        {
            Contract.Requires<ArgumentNullException>(theParseTree != null);
            Errors = new List<string>();
            Status = theParseStatus;
            Tree = theParseTree;
            Root = (ConstraintExpressionNode) theParseTree.Root.AstNode;
        }

        public ConstraintExpressionParseResult(ConstraintExpressionParseStatus theParseStatus, IEnumerable<string> theErrors)
        {
            Contract.Requires<ArgumentNullException>(theErrors != null);
            Errors = theErrors.ToList();
            Status = theParseStatus;
        }

        public IReadOnlyCollection<string> Errors { get; private set; }
        public ConstraintExpressionParseStatus Status { get; private set; }
        public ConstraintExpressionNode Root { get; private set; }
        public ParseTree Tree { get; private set; }
    }
}