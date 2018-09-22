using Irony.Parsing;
using Workbench.Core.Nodes;

namespace Workbench.Core.Grammars
{
    /// <summary>
    /// Grammar for variable inline domain expressions.
    /// </summary>
    [Language("Variable Domain Expression Grammar", "0.1", "A grammar for variable inline domain expressions.")]
    internal class VariableDomainGrammar : Grammar
    {
        internal VariableDomainGrammar()
            : base(caseSensitive: false)
        {
            LanguageFlags = LanguageFlags.CreateAst |
                            LanguageFlags.NewLineBeforeEOF;

            var RANGE = ToTerm("..", "range");
            var COMMA = ToTerm(",");
            var OPEN_ARG = ToTerm("(");
            var CLOSE_ARG = ToTerm(")");
            var TABLE_COLUMN_SPECIFIER = ToTerm("!");
            var TABLE_INDEX_SEPERATOR = ToTerm(":");

            // Terminals
            var numberLiteral = new NumberLiteral("number literal", NumberOptions.IntOnly, typeof (NumberLiteralNode));
            var characterLiteral = new StringLiteral("character literal", "'", StringOptions.IsChar);
            characterLiteral.AstConfig.NodeType = typeof(CharacterLiteralNode);
            var item = new StringLiteral("string literal", "\"", StringOptions.None);
            item.AstConfig.NodeType = typeof(ItemNameNode);
            var functionCallArgumentStringLiteral = new IdentifierTerminal("function call argument string literal");
            functionCallArgumentStringLiteral.AstConfig.NodeType = typeof (FunctionCallArgumentStringLiteralNode);
            var functionName = new IdentifierTerminal("function name");
            functionName.AstConfig.NodeType = typeof (FunctionNameNode);
            var domainName = new IdentifierTerminal("domain name");
            domainName.AddPrefix("$", IdOptions.NameIncludesPrefix);
            domainName.AstConfig.NodeType = typeof(DomainNameNode);
            var tableCellReference = new IdentifierTerminal("table cell reference");
            tableCellReference.AstConfig.NodeType = typeof(TableCellReferenceNode);
            var cellReference = new IdentifierTerminal("cell reference", IdOptions.IsNotKeyword);
            cellReference.AstConfig.NodeType = typeof(TableReferenceNode);

            // Non-terminals
            var domainExpression = new NonTerminal("domainExpression", typeof (VariableDomainExpressionNode));
            var rangeDomainExpression = new NonTerminal("range domain expression", typeof(RangeDomainExpressionNode));
            var itemsList = new NonTerminal("list items", typeof(ItemsListNode));
            var listDomainExpression = new NonTerminal("list domain expression", typeof(ListDomainExpressionNode));
            var bandExpression = new NonTerminal("expression", typeof (BandExpressionNode));
            var functionCall = new NonTerminal("function call", typeof (FunctionInvocationNode));
            var functionCallArgumentList = new NonTerminal("function call arguments", typeof (FunctionArgumentListNode));
            var functionCallArgument = new NonTerminal("function argument", typeof (FunctionCallArgumentNode));
            var sharedDomainReference = new NonTerminal("shared domain reference", typeof(SharedDomainReferenceNode));
            var tableExpression = new NonTerminal("table range", typeof(TableExpressionNode));
            var cellExpression = new NonTerminal("cell expression", typeof(CellExpressionNode));
            var cellRangeExpression = new NonTerminal("cell range", typeof(TableRangeExpressionNode));
            var cellListExpression = new NonTerminal("cell list", typeof(TableListExpressionNode));

            // BNF rules
            itemsList.Rule = MakePlusRule(itemsList, COMMA, item);
            listDomainExpression.Rule = itemsList;
            functionCallArgument.Rule = numberLiteral | functionCallArgumentStringLiteral;
            functionCall.Rule = functionName + OPEN_ARG + functionCallArgumentList + CLOSE_ARG;
            functionCallArgumentList.Rule = MakePlusRule(functionCallArgumentList, COMMA, functionCallArgument);
            bandExpression.Rule = numberLiteral | functionCall | characterLiteral;
            rangeDomainExpression.Rule = bandExpression + RANGE + bandExpression;
            sharedDomainReference.Rule = domainName;
            cellRangeExpression.Rule = tableCellReference + TABLE_INDEX_SEPERATOR + tableCellReference;
            cellListExpression.Rule = MakePlusRule(cellListExpression, COMMA, tableCellReference);
            cellExpression.Rule = cellRangeExpression | cellListExpression;
            tableExpression.Rule = cellReference + TABLE_COLUMN_SPECIFIER + cellExpression;
            domainExpression.Rule = NewLine | rangeDomainExpression | sharedDomainReference| listDomainExpression | tableExpression;

            Root = domainExpression;

            MarkPunctuation(RANGE, COMMA);
            MarkPunctuation(OPEN_ARG, CLOSE_ARG);
            MarkPunctuation(TABLE_COLUMN_SPECIFIER, TABLE_INDEX_SEPERATOR);
            MarkTransient(cellExpression);

            RegisterBracePair("(", ")");
        }
    }
}