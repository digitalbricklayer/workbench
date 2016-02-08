using System.Windows;
using Caliburn.Micro;
using Moq;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class VariableVisualizerViewModelTests
    {
        [Test]
        public void CreateVisualizerWithVariableBindingSetsSelectedVariable()
        {
            var sut = CreateSut();
            Assert.That(sut.SelectedVariable, Is.EqualTo("x"));
        }

        private VariableVisualizerDesignViewModel CreateSut()
        {
            var visualizerModel = new VariableVisualizerModel(new Point());
            visualizerModel.BindTo(new VariableModel("x"));
            return new VariableVisualizerDesignViewModel(visualizerModel,
                                                         Mock.Of<IEventAggregator>(),
                                                         Mock.Of<IDataService>());
        }
    }
}
