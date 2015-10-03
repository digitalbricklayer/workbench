using Sprache;

namespace Dyna.Core.Models
{
    /// <summary>
    /// Grammar for parsing a constraint expression.
    /// </summary>
    internal class ConstraintGrammar
    {
        /// <summary>
        /// Parse an identifier.
        /// </summary>
        private static readonly Parser<string> identifier =
            from first in Sprache.Parse.Letter.Once().Text()
            from rest in Sprache.Parse.LetterOrDigit.Many().Text()
            select string.Concat(first, rest);

        /// <summary>
        /// Parse a literal.
        /// </summary>
        private static readonly Parser<string> literal =
            from first in Sprache.Parse.Number
            select first;

        /// <summary>
        /// LHS must be a variable.
        /// </summary>
        private static readonly Parser<VariableModel> leftHandSide =
            from leading in Sprache.Parse.WhiteSpace.Many()
            from id in identifier
            from trailing in Sprache.Parse.WhiteSpace.Many()
            select new VariableModel(id);

        /// <summary>
        /// Parse an expression, either a variable or a literal.
        /// </summary>
        private static readonly Parser<Expression> expression =
            identifier.Select(Expression.CreateIdentifier)
                .Or(literal.Select(Expression.CreateLiteral));

        /// <summary>
        /// RHS can be a variable or integer literal.
        /// </summary>
        private static readonly Parser<Expression> rightHandSide =
            from leading in Sprache.Parse.WhiteSpace.Many()
            from exp in expression
            from trailing in Sprache.Parse.WhiteSpace.Many()
            select exp;

        /// <summary>
        /// Operator parser.
        /// </summary>
        private static readonly Parser<OperatorType> op =
            from leading in Sprache.Parse.WhiteSpace.Many()
            from type in operatorType
            from trailing in Sprache.Parse.WhiteSpace.Many()
            select type;

        /// <summary>
        /// Operator type parser.
        /// </summary>
        private static readonly Parser<OperatorType> operatorType =
            from type in Sprache.Parse.String("=").Return(OperatorType.Equals)
                .Or(Sprache.Parse.String("!=").Return(OperatorType.NotEqual))
                .Or(Sprache.Parse.String(">=").Return(OperatorType.GreaterThanOrEqual))
                .Or(Sprache.Parse.String("<=").Return(OperatorType.LessThanOrEqual))
                .Or(Sprache.Parse.String(">").Return(OperatorType.Greater))
                .Or(Sprache.Parse.String("<").Return(OperatorType.Less))
            select type;

        /// <summary>
        /// Parse a raw constraint expression.
        /// </summary>
        /// <param name="rawExpression">String constraint expression.</param>
        /// <returns>Parsed expression tree.</returns>
        public static ConstraintExpressionUnit Parse(string rawExpression)
        {
            var constraintGrammar =
                from lhs in leftHandSide
                from operatorType in op
                from rhs in rightHandSide
                select new ConstraintExpressionUnit(lhs, rhs, operatorType);

            return constraintGrammar.End().Parse(rawExpression);
        }
    }
}
