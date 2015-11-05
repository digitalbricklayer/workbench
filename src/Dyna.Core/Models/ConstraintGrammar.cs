using System;
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
        private static readonly Parser<VariableModel> Identifier =
            from first in Sprache.Parse.Letter.Once().Text()
            from rest in Sprache.Parse.LetterOrDigit.Many().Text()
            select new VariableModel(string.Concat(first, rest));

        /// <summary>
        /// Parse a literal.
        /// </summary>
        private static readonly Parser<string> Literal =
            from first in Sprache.Parse.Number
            select first;

        /// <summary>
        /// Parse an aggregate variable reference.
        /// </summary>
        private static readonly Parser<AggregateVariableReference> AggregateVariableReference =
            from variableName in Identifier
            from openingSubscript in Sprache.Parse.Char('[').Once()
            from subscriptStatement in Sprache.Parse.Number.Text()
            from closingSubscript in Sprache.Parse.Char(']').Once()
            select new AggregateVariableReference(variableName.Name,
                                                  Convert.ToInt32(subscriptStatement));

        /// <summary>
        /// Parse a singleton variable name.
        /// </summary>
        private static readonly Parser<VariableModel> SingletonVariableReference =
            from leading in Sprache.Parse.WhiteSpace.Many()
            from variable in Identifier
            from trailing in Sprache.Parse.WhiteSpace.Many()
            select variable;

        /// <summary>
        /// Parse an expression, either a singleton variable reference, an 
        /// aggregate variable reference or a literal.
        /// </summary>
        private static readonly Parser<Expression> Expression =
            AggregateVariableReference.Select(Models.Expression.CreateAggregateReference)
                .Or(SingletonVariableReference.Select(Models.Expression.CreateSingletonReference))
                .Or(Literal.Select(Models.Expression.CreateLiteral));

        /// <summary>
        /// LHS can be a singleton variable name or an aggregate variable reference.
        /// </summary>
        private static readonly Parser<Expression> LeftHandSide =
            AggregateVariableReference.Select(Models.Expression.CreateAggregateReference)
                .Or(SingletonVariableReference.Select(Models.Expression.CreateSingletonReference));

        /// <summary>
        /// RHS can be a singleton variable, an aggregate variable 
        /// reference or an integer literal.
        /// </summary>
        private static readonly Parser<Expression> RightHandSide =
            from leading in Sprache.Parse.WhiteSpace.Many()
            from exp in Expression
            from trailing in Sprache.Parse.WhiteSpace.Many()
            select exp;

        /// <summary>
        /// Operator parser.
        /// </summary>
        private static readonly Parser<OperatorType> Operator =
            from leading in Sprache.Parse.WhiteSpace.Many()
            from type in OperatorType
            from trailing in Sprache.Parse.WhiteSpace.Many()
            select type;

        /// <summary>
        /// Operator type parser.
        /// </summary>
        private static readonly Parser<OperatorType> OperatorType =
            from type in Sprache.Parse.String("=").Return(Models.OperatorType.Equals)
                .Or(Sprache.Parse.String("!=").Return(Models.OperatorType.NotEqual))
                .Or(Sprache.Parse.String(">=").Return(Models.OperatorType.GreaterThanOrEqual))
                .Or(Sprache.Parse.String("<=").Return(Models.OperatorType.LessThanOrEqual))
                .Or(Sprache.Parse.String(">").Return(Models.OperatorType.Greater))
                .Or(Sprache.Parse.String("<").Return(Models.OperatorType.Less))
            select type;

        /// <summary>
        /// Parse a raw constraint expression.
        /// </summary>
        /// <param name="rawExpression">String constraint expression.</param>
        /// <returns>Parsed expression tree.</returns>
        public static ConstraintExpressionUnit Parse(string rawExpression)
        {
            var constraintGrammar =
                from lhs in LeftHandSide
                from operatorType in Operator
                from rhs in RightHandSide
                select new ConstraintExpressionUnit(lhs, rhs, operatorType);

            return constraintGrammar.End().Parse(rawExpression);
        }
    }
}
