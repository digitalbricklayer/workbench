using Caliburn.Micro;
using Moq;
using Workbench.ViewModels;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Nodes;
using Workbench.Services;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class ExpressionConstraintEditorViewModelTests
    {
        [Test]
        public void IsValid_With_Empty_Expression_Returns_False()
        {
            var sut = new ExpressionConstraintEditorViewModel(new ExpressionConstraintGraphicModel(new ExpressionConstraintModel()), CreateEventAggregator(), CreateDataService(), CreateViewModelService());
            Assert.That(sut.IsValid, Is.False);
        }

        [Test]
        public void IsValid_With_Valid_Expression_Returns_True()
        {
            var sut = new ExpressionConstraintEditorViewModel(new ExpressionConstraintGraphicModel(new ExpressionConstraintModel(new ModelName("X"), new ConstraintExpressionModel("$X < $Y"))), CreateEventAggregator(), CreateDataService(), CreateViewModelService());
            Assert.That(sut.IsValid, Is.True);
        }

        [Test]
        public void UpdateConstraintExpressionTextUpdatesExpressionModel()
        {
            var sut = new ExpressionConstraintEditorViewModel(new ExpressionConstraintGraphicModel(new ExpressionConstraintModel()), CreateEventAggregator(), CreateDataService(), CreateViewModelService());
            sut.Expression.Text = "$x > 1";
            var leftVariableReference = (SingletonVariableReferenceNode)sut.Expression.Model.Node.InnerExpression.LeftExpression.InnerExpression;
            Assert.That(leftVariableReference.VariableName, Is.EqualTo("x"));
        }

        [Test]
        public void UpdateConstraintExpressionTextUpdatesConstraintModel()
        {
            var sut = new ExpressionConstraintEditorViewModel(new ExpressionConstraintGraphicModel(new ExpressionConstraintModel()), CreateEventAggregator(), CreateDataService(), CreateViewModelService());
            sut.Expression.Text = "$x > 1";
            var leftVariableReference = (SingletonVariableReferenceNode)sut.Model.Expression.Node.InnerExpression.LeftExpression.InnerExpression;
            var leftVariableReference2 = (SingletonVariableReferenceNode)sut.Expression.Model.Node.InnerExpression.LeftExpression.InnerExpression;
            Assert.That(leftVariableReference.VariableName, Is.EqualTo(leftVariableReference2.VariableName));
        }

        private static IDataService CreateDataService()
        {
            return new DataService(Mock.Of<IWorkspaceReaderWriter>());
        }

        private static IViewModelFactory CreateViewModelFactory()
        {
            return new ViewModelFactory(CreateEventAggregator(), CreateWindowManager());
        }

        private static IViewModelService CreateViewModelService()
        {
            return new ViewModelService(CreateViewModelFactory());
        }

        private static IEventAggregator CreateEventAggregator()
        {
            return new Mock<IEventAggregator>().Object;
        }

        private static IWindowManager CreateWindowManager()
        {
            return new Mock<IWindowManager>().Object;
        }
    }
}
