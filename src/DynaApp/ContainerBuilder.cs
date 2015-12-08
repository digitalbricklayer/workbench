using Caliburn.Micro;
using Dyna.Core.Models;
using DynaApp.Factories;
using DynaApp.Services;
using DynaApp.ViewModels;

namespace DynaApp
{
    internal static class ContainerBuilder
    {
        /// <summary>
        /// Build the application container.
        /// </summary>
        /// <returns>A populated container.</returns>
        internal static SimpleContainer Build()
        {
            var container = new SimpleContainer();

            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.Singleton<DataService>();
            container.PerRequest<IWorkspaceReaderWriter, BinaryFileWorkspaceReaderWriter>();
            container.PerRequest<IWorkspaceReader, BinaryFileWorkspaceReader>();
            container.PerRequest<IWorkspaceWriter, BinaryFileWorkspaceWriter>();
            container.PerRequest<IViewModelFactory, SimpleContainerViewModelFactory>();
            container.PerRequest<MainWindowViewModel>();
            container.PerRequest<WorkspaceViewModel>();
            container.PerRequest<ConstraintViewModel>();
            container.PerRequest<DomainViewModel>();
            container.PerRequest<VariableViewModel>();
            container.PerRequest<AggregateVariableViewModel>();
            container.PerRequest<AggregateResizeViewModel>();
            container.PerRequest<ConstraintExpressionViewModel>();
            container.PerRequest<DomainExpressionViewModel>();
            container.PerRequest<ModelErrorsViewModel>();
            container.PerRequest<ModelErrorViewModel>();
            container.PerRequest<ModelViewModel>();
            container.PerRequest<SolutionViewModel>();
            container.PerRequest<VariableDomainExpressionViewModel>();
            container.PerRequest<ValueViewModel>();

            return container;
        }
    }
}
