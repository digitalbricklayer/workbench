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
