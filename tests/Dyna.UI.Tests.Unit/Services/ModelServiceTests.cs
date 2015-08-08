using DynaApp.Services;
using NUnit.Framework;

namespace Dyna.UI.Tests.Unit.Services
{
    [TestFixture]
    public class ModelServiceTests
    {
        [Test]
        [Ignore("The model should not be mappable from the view model. See #42 on GitHub.")]
        public void MapFrom_With_Valid_View_Model_Returns_Expected_Model()
        {
            var sut = new ModelService();
            var actualWorkspaceModel = sut.MapFrom(WorkspaceViewModelFactory.Create());
            Assert.That(actualWorkspaceModel, Is.Not.Null);
        }
    }
}
