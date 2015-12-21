using Caliburn.Micro;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench
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
            container.Singleton<IDataService, DataService>();
            container.PerRequest<IWorkspaceReaderWriter, BinaryFileWorkspaceReaderWriter>();
            container.PerRequest<IWorkspaceReader, BinaryFileWorkspaceReader>();
            container.PerRequest<IWorkspaceWriter, BinaryFileWorkspaceWriter>();
            container.PerRequest<IShell, ShellViewModel>();

            return container;
        }
    }
}
