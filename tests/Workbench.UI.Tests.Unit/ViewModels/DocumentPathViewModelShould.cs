using NUnit.Framework;
using Workbench.ViewModels;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class DocumentPathViewModelShould
    {
        [Test]
        public void ReturnTrueWhenCreatedWithDefaultConstructor()
        {
            var sut = new DocumentPathViewModel();
            Assert.IsTrue(sut.IsEmpty);
        }

        [Test]
        public void ReturnTrueWhenCreatedWithPath()
        {
            var sut = new DocumentPathViewModel(@"c:\a file.dps");
            Assert.IsFalse(sut.IsEmpty);
        }
    }
}
