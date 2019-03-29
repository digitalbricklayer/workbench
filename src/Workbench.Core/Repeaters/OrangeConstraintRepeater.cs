using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using Workbench.Core.Nodes;
using Workbench.Core.Parsers;
using Workbench.Core.Solvers;

namespace Workbench.Core.Repeaters
{
    /// <summary>
    /// Process a constraint repeater by expanding the expression an 
    /// appropriate number of times.
    /// </summary>
    internal class OrangeConstraintRepeater
    {
        private OrangeConstraintRepeaterContext _context;
        private readonly OrangeModelSolverMap _modelSolverMap;
        private readonly ModelModel _model;
        private readonly ValueMapper _valueMapper;
        private readonly ConstraintNetwork _constraintNetwork;

        internal OrangeConstraintRepeater(ConstraintNetwork constraintNetwork, OrangeModelSolverMap modelSolverMap, ModelModel theModel, ValueMapper theValueMapper)
        {
            Contract.Requires<ArgumentNullException>(constraintNetwork != null);
            Contract.Requires<ArgumentNullException>(modelSolverMap != null);
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(theValueMapper != null);

            _constraintNetwork = constraintNetwork;
            _modelSolverMap = modelSolverMap;
            _model = theModel;
            _valueMapper = theValueMapper;
        }

        internal void Process(OrangeConstraintRepeaterContext context)
        {
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Assert(context.HasRepeaters);

            _context = context;
            var constraintExpressionParser = new ConstraintExpressionParser();
            var expressionTemplateWithoutExpanderText = StripExpanderFrom(context.Constraint.Expression.Text);
            while (context.Next())
            {
                var expressionText = InsertCounterValuesInto(expressionTemplateWithoutExpanderText);
                var expandedConstraintExpressionResult = constraintExpressionParser.Parse(expressionText);
                ProcessConstraint(expandedConstraintExpressionResult.Root);
            }
        }

        internal OrangeConstraintRepeaterContext CreateContextFrom(ExpressionConstraintModel constraint)
        {
            return new OrangeConstraintRepeaterContext(constraint, _model);
        }

        private void ProcessConstraint(ConstraintExpressionNode constraintExpressionNode)
        {
            Contract.Requires<ArgumentNullException>(constraintExpressionNode != null);

            var newArc = new ArcBuilder(_modelSolverMap).Build(constraintExpressionNode);
            _constraintNetwork.AddArc(newArc);
        }

        private string StripExpanderFrom(string expressionText)
        {
            var expanderKeywordPos = expressionText.IndexOf("|", StringComparison.Ordinal);
            var raw = expressionText.Substring(0, expanderKeywordPos);
            return raw.Trim();
        }

        private string InsertCounterValuesInto(string expressionTemplateText)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(expressionTemplateText));

            var accumulatingTemplateText = expressionTemplateText;
            foreach (var aCounter in _context.Counters)
            {
                accumulatingTemplateText = InsertCounterValueInto(accumulatingTemplateText,
                                                                  aCounter.CounterName,
                                                                  aCounter.CurrentValue);
            }

            return accumulatingTemplateText;
        }

        private string InsertCounterValueInto(string expressionTemplateText, string counterName, int counterValue)
        {
            return expressionTemplateText.Replace(counterName, Convert.ToString(counterValue));
        }
    }
}
