using Workbench.ViewModels;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class ExpressionConstraintViewModelTests
    {
        [Test]
        public void IsValid_With_Empty_Expression_Returns_False()
        {
            var sut = new ExpressionConstraintViewModel(new ExpressionConstraintGraphicModel("X", string.Empty));
            Assert.That(sut.IsValid, Is.False);
        }

        [Test]
        public void IsValid_With_Valid_Expression_Returns_True()
        {
            var sut = new ExpressionConstraintViewModel(new ExpressionConstraintGraphicModel("X", "X < Y"));
            Assert.That(sut.IsValid, Is.True);
        }

        [Test]
        public void UpdateConstraintExpressionTextUpdatesExpressionModel()
        {
            var sut = new ExpressionConstraintViewModel(new ExpressionConstraintGraphicModel());
            sut.Expression.Text = "x > 1";
            var leftVariableReference = (SingletonVariableReferenceNode)sut.Expression.Model.Node.InnerExpression.LeftExpression.InnerExpression;
            Assert.That(leftVariableReference.VariableName, Is.EqualTo("x"));
        }

        [Test]
        public void UpdateConstraintExpressionTextUpdatesConstraintModel()
        {
            var sut = new ExpressionConstraintViewModel(new ExpressionConstraintGraphicModel());
            sut.Expression.Text = "x > 1";
            var leftVariableReference = (SingletonVariableReferenceNode)sut.Model.Expression.Node.InnerExpression.LeftExpression.InnerExpression;
            var leftVariableReference2 = (SingletonVariableReferenceNode)sut.Expression.Model.Node.InnerExpression.LeftExpression.InnerExpression;
            Assert.That(leftVariableReference.VariableName, Is.EqualTo(leftVariableReference2.VariableName));
        }
    }
}
