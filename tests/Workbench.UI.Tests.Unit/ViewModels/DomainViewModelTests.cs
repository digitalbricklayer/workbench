using System;
using Workbench.ViewModels;
using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class DomainViewModelTests
    {
        [Test]
        public void IsValid_With_Valid_Expression_Returns_True()
        {
            var sut = new DomainViewModel(new DomainModel("X", "1..2"));
            Assert.That(sut.IsValid, Is.True);
        }

        [Test]
        public void IsValid_With_Empty_Expression_Returns_False()
        {
            var sut = new DomainViewModel(new DomainModel("X", string.Empty));
            Assert.That(sut.IsValid, Is.False);
        }

        [Test]
        public void UpdateDomainExpressionUpdatesModel()
        {
            var sut = new DomainViewModel(new DomainModel());
            sut.Expression.Text = "1..10";
            Assert.That(sut.Expression.Model.Node, Is.Not.Null);
        }
    }
}
