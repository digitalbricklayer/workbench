using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Irony.Parsing;

namespace Workbench.Core.Parsers
{
    public class ParseResult<T>
    {
        public ParseResult(ParseStatus theParseStatus, ParseTree theParseTree)
        {
            Contract.Requires<ArgumentNullException>(theParseTree != null);
            Errors = new List<string>();
            Status = theParseStatus;
            Tree = theParseTree;
            Root = (T) theParseTree.Root.AstNode;
        }

        public ParseResult(ParseStatus theParseStatus, IEnumerable<string> theErrors)
        {
            Contract.Requires<ArgumentNullException>(theErrors != null);
            Errors = theErrors.ToList();
            Status = theParseStatus;
        }

        /// <summary>
        /// Gets a list of errors found whilst parsing the expression.
        /// </summary>
        public IReadOnlyCollection<string> Errors { get; private set; }

        /// <summary>
        /// Gets the parse status.
        /// </summary>
        public ParseStatus Status { get; private set; }

        /// <summary>
        /// Gets the root AST node.
        /// </summary>
        public T Root { get; private set; }

        /// <summary>
        /// Gets the Irony parse tree.
        /// </summary>
        public ParseTree Tree { get; private set; }

        /// <summary>
        /// Gets whether the parse was successful.
        /// </summary>
        public bool IsSuccess => Status == ParseStatus.Success;
    }
}
