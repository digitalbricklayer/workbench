using System.Linq;
using DynaApp.Models;
using NUnit.Framework;

namespace Dyna.UI.Tests.Unit.Models
{
    [TestFixture]
    public class ModelModelTests
    {
        private DomainModel domain;
        private VariableModel variable;

        [Test]
        public void Connect_Variable_To_Domain_Adds_Connection()
        {
            var sut = CreateSut();
            sut.Connect(this.variable, this.domain);

            var actualNewConnection = sut.Connections.First();
            Assert.That(actualNewConnection.DestinationConnector.Parent, Is.SameAs(this.variable));
            Assert.That(actualNewConnection.SourceConnector.Parent, Is.SameAs(this.domain));
        }

        private ModelModel CreateSut()
        {
            var theModel = new ModelModel();
            this.variable = new VariableModel("x");
            theModel.AddVariable(this.variable);
            this.domain = new DomainModel("a domain", "1..10");
            theModel.AddDomain(this.domain);

            return theModel;
        }
    }
}
