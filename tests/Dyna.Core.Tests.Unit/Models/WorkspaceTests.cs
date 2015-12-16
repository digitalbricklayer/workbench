using FluentAssertions;
using NUnit.Framework;

namespace Dyna.Core.Tests.Unit.Models
{
    [TestFixture]
    public class WorkspaceTests
    {
        [Test]
        public void Workspace_Is_Serializable()
        {
            var sut = WorkspaceModelFactory.Create();
            sut.Should().BeBinarySerializable();
        }
    }
}
