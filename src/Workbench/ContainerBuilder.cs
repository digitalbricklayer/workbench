using Caliburn.Micro;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench
{
    /// <summary>
    /// Application container builder.
    /// </summary>
    internal static class ContainerBuilder
    {
        /// <summary>
        /// Build the application container.
        /// </summary>
        /// <returns>A populated container.</returns>
        internal static SimpleContainer Build()
        {
            var container = new SimpleContainer();

            container.Singleton<IAppRuntime, AppRuntime>();
            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.Singleton<IDataService, DataService>();
            container.PerRequest<IViewModelFactory, ViewModelFactory>();
            container.PerRequest<WorkspaceMapper>();
            container.PerRequest<IWorkspaceReaderWriter, BinaryFileWorkspaceReaderWriter>();
            container.PerRequest<IWorkspaceReader, BinaryFileWorkspaceReader>();
            container.PerRequest<IWorkspaceWriter, BinaryFileWorkspaceWriter>();
            container.Singleton<IShell, ShellViewModel>();
            container.Singleton<WorkspaceViewModel>();
            container.Singleton<ApplicationMenuViewModel>();
            container.Singleton<TitleBarViewModel>();

            return container;
        }
    }
}
