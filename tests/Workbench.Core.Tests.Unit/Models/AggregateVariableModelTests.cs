using System;
using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class AggregateVariableModelTests
    {
        [Test]
        public void InitializeAggregateWithValidNameSetsExpectedNameInVariable()
        {
            var sut = new AggregateVariableModel("x");
            Assert.That(sut.Name, Is.EqualTo("x"));
        }

        [Test]
        public void InitializeVariableWithEmptyExpressionWoutWhitespace()
        {
            var sut = new AggregateVariableModel("x", 5, "");
            Assert.That(sut.DomainExpression.IsEmpty, Is.True);
        }

        [Test]
        public void InitializeVariableWithDomainReferenceRawExpressionWithWhitespace()
        {
            var sut = new AggregateVariableModel("x", 2, "   A    ");
            Assert.That(sut.DomainExpression.DomainReference.DomainName.Name, Is.EqualTo("A"));
        }

        [Test]
        public void InitializeVariableWithDomainReferenceRawExpressionWoutWhitespace()
        {
            var sut = new AggregateVariableModel("x", 1, "A");
            Assert.That(sut.DomainExpression.DomainReference.DomainName.Name, Is.EqualTo("A"));
        }

        [Test]
        public void InitializeVariableWithInlineRawExpressionWoutWhitespace()
        {
            var sut = new AggregateVariableModel("x", 10, "1..10");
            Assert.That(sut.DomainExpression.InlineDomain, Is.Not.Null);
        }

        [Test]
        public void ChangeDomainOfAggregatedVariableWithValueInsideAggregateDomain()
        {
            var sut = new AggregateVariableModel("A test", 10, "1..10");
            sut.Resize(10);
            sut.OverrideDomainTo(9, new VariableDomainExpressionModel("1..5"));
            var actualVariable = sut.GetVariableByIndex(9);
            Assert.That(actualVariable.DomainExpression.Text, Is.EqualTo("1..5"));
        }

        [Test]
        [Ignore("Need to add validation. But probably not this way")]
        public void ChangeDomainOfAggregatedVariableWithValueOutsideAggregateDomain()
        {
            var sut = new AggregateVariableModel("A test", 2, "1..10");
            sut.Resize(10);
            sut.OverrideDomainTo(9, new VariableDomainExpressionModel("1..5"));
            Assert.Throws<ArgumentException>(() => sut.OverrideDomainTo(9, new VariableDomainExpressionModel("8..11")));
        }

        [Test]
        public void ChangeDomainOfAggregatedVariableWithValueOutsideAggregateDomainX()
        {
            var sut = new AggregateVariableModel("A test", 1, "1..10");
            sut.Resize(10);
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.OverrideDomainTo(10, new VariableDomainExpressionModel("8..10")));
        }

        [Test]
        public void ResizeAggregateWithZeroThrowsArgumentOutOfRangeException()
        {
            var sut = new AggregateVariableModel("x");
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Resize(0));
        }

        [Test]
        public void ResizeAggregateWithTenSetsNewSize()
        {
            var sut = new AggregateVariableModel("x");
            sut.Resize(10);
            Assert.That(sut.AggregateCount, Is.EqualTo(10));
        }

        [Test]
        public void ResizeAggregateToGreaterSIzeAllVariablesNotNull()
        {
            var sut = new AggregateVariableModel("x");
            sut.Resize(10);
            Assert.That(sut.Variables, Is.All.InstanceOf<VariableModel>());
        }

        [Test]
        public void ResizeAggregateToSmallerSizeAllVariablesNotNull()
        {
            var sut = new AggregateVariableModel("x");
            sut.Resize(10);
            sut.Resize(5);
            Assert.That(sut.Variables, Is.All.InstanceOf<VariableModel>());
        }
    }
}
