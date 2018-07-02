using System;
using Caliburn.Micro;
using Moq;
using Workbench.ViewModels;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Messages;
using Workbench.Services;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class SingletonVariableEditorViewModelTests
    {
        private readonly Mock<IEventAggregator> eventAggregatorMock = new Mock<IEventAggregator>();

        [Test]
        public void RenameVariableWithValidNewNamePublishesVariableRenamedMessage()
        {
            var sut = CreateVariable();
            sut.Name = "NewName";
            this.eventAggregatorMock.Verify(_ => _.Publish(It.Is<VariableRenamedMessage>(message => message.Renamed.Name == sut.SingletonVariableGraphic.Name), It.IsAny<Action<System.Action>>()),
                                            Times.Once);
        }

        [Test]
        public void UpdateVariableDomainExpressionWithDomainReferenceUpdatesModel()
        {
            var sut = CreateVariable();
            sut.DomainExpression.Text = "$x";
            Assert.That(sut.DomainExpression.Model.DomainReference.DomainName.Name, Is.EqualTo("x"));
        }

        [Test]
        public void UpdateDomainExpressionWithInlineDomainUpdatesModel()
        {
            var sut = CreateVariable();
            sut.DomainExpression.Text = "1..10";
            Assert.That(sut.DomainExpression.Model.InlineDomain, Is.Not.Null);
        }

        private SingletonVariableEditorViewModel CreateVariable()
        {
            return new SingletonVariableEditorViewModel(new SingletonVariableGraphicModel(new SingletonVariableModel(new ModelModel(), new ModelName("X"))),
                                                        this.eventAggregatorMock.Object, CreateDataService(), CreateViewModelService());
        }

        private IDataService CreateDataService()
        {
            return new DataService(Mock.Of<IWorkspaceReaderWriter>());
        }

        private IViewModelFactory CreateViewModelFactory()
        {
            return new ViewModelFactory(this.eventAggregatorMock.Object, CreateWindowManager());
        }

        private IViewModelService CreateViewModelService()
        {
            return new ViewModelService();
        }

        private IWindowManager CreateWindowManager()
        {
            return new Mock<IWindowManager>().Object;
        }
    }
}
