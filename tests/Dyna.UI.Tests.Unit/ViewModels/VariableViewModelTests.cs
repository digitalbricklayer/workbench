using DynaApp.ViewModels;
using NUnit.Framework;

namespace Dyna.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class VariableViewModelTests
    {
        [Test]
        public void Variable_Cannot_Connect_To_Another_Variable()
        {
            // Arrange
            var sut = CreateVariable();

            // Act
            var actualResult= sut.IsConnectableTo(new VariableViewModel("Y"));

            // Assert
            Assert.That(actualResult, Is.False);
        }

        [Test]
        public void Variable_Can_Connect_To_A_Constraint()
        {
            // Arrange
            var sut = CreateVariable();

            // Act
            var actualResult = sut.IsConnectableTo(new ConstraintViewModel("Y"));

            // Assert
            Assert.That(actualResult, Is.True);
        }

        [Test]
        public void Variable_Can_Connect_To_A_Domain()
        {
            // Arrange
            var sut = CreateVariable();

            // Act
            var actualResult = sut.IsConnectableTo(new DomainViewModel("Z"));

            // Assert
            Assert.That(actualResult, Is.True);
        }

        [Test]
        public void Variable_Cannot_Connect_To_The_Same_Domain_More_Than_Once()
        {
            // Arrange
            var model = new ModelViewModel();
            var sut = CreateVariable();
            model.AddVariable(sut);
            var aDomain = CreateDomain();
            model.AddDomain(aDomain);
            model.Connect(sut, aDomain);

            // Act
            var actualResult = sut.IsConnectableTo(aDomain);

            // Assert
            Assert.That(actualResult, Is.False);
        }

        [Test]
        public void Variable_Cannot_Connect_To_The_Same_Constraint_More_Than_Once()
        {
            // Arrange
            var model = new ModelViewModel();
            var sut = CreateVariable();
            model.AddVariable(sut);
            var aConstraint = CreateConstraint();
            model.AddConstraint(aConstraint);
            model.Connect(sut, aConstraint);

            // Act
            var actualResult = sut.IsConnectableTo(aConstraint);

            // Assert
            Assert.That(actualResult, Is.False);
        }

        private static ConstraintViewModel CreateConstraint()
        {
            var y = new ConstraintViewModel("Y");
            y.AddConnector(new ConnectorViewModel());
            y.AddConnector(new ConnectorViewModel());

            return y;
        }

        private static DomainViewModel CreateDomain()
        {
            var y = new DomainViewModel("Y");
            y.AddConnector(new ConnectorViewModel());
            y.AddConnector(new ConnectorViewModel());

            return y;
        }

        private static VariableViewModel CreateVariable()
        {
            var x = new VariableViewModel("X");
            x.AddConnector(new ConnectorViewModel());
            x.AddConnector(new ConnectorViewModel());

            return x;
        }
    }
}
