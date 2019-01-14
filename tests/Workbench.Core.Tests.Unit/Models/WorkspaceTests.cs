using NUnit.Framework;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class WorkspaceTests
    {
        [Test]
        public void CreateWorkspaceWithBlankNameReturnsModelWithBlankName()
        {
            var sut = WorkspaceModelFactory.Create();
            Assert.That(sut.Model.Name.Text, Is.Empty);
        }

        [Test]
        public void Create_Workspace_With_Name_Returns_Model_With_Expected_Name()
        {
            const string expectedModelName = "The expected model name";
            var sut = new WorkspaceBuilder(expectedModelName)
                                    .AddSingleton("x", "1..10")
                                    .WithConstraintExpression("x > 1")
                                    .Build();
            Assert.That(sut.Model.Name.Text, Is.EqualTo(expectedModelName));
        }
    }
}
