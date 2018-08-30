using Caliburn.Micro;
using Moq;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class ExpressionConstraintModelItemViewModelTests
    {
        [Test]
        public void Initialize_With_Default_Values_Text_Is_Empty()
        {
            var sut = new ExpressionConstraintModelItemViewModel(new ExpressionConstraintModel(), Mock.Of<IWindowManager>());
            Assert.That(sut.ExpressionText, Is.Empty);
        }
    }
}
