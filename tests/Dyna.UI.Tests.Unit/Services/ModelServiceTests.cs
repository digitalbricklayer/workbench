using DynaApp.Services;
using NUnit.Framework;

namespace Dyna.UI.Tests.Unit.Services
{
    [TestFixture]
    public class ModelServiceTests
    {
        [Test]
        public void MapFrom_With_Valid_Model_Returns_Expected_View_Model()
        {
            var sut = new WorkspaceMapper();
            var actualWorkspaceModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel, Is.Not.Null);
        }
    }
}
