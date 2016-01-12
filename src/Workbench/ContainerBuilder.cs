using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
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
        internal static WindsorContainer Build()
        {
            var container = new WindsorContainer();

            container.Register(Component.For<IAppRuntime, AppRuntime>()
                                        .LifeStyle.Singleton,
                               Component.For<IWindowManager, WindowManager>()
                                        .LifeStyle.Singleton,
                               Component.For<IEventAggregator, EventAggregator>()
                                        .LifeStyle.Singleton,
                               Component.For<IDataService, DataService>()
                                        .LifeStyle.Singleton,
                               Component.For<IShell, ShellViewModel>()
                                        .LifeStyle.Singleton,
                               Component.For<WorkspaceViewModel>()
                                        .LifeStyle.Singleton,
                               Component.For<ApplicationMenuViewModel>()
                                        .LifeStyle.Singleton,
                               Component.For<TitleBarViewModel>()
                                        .LifeStyle.Singleton,
                               Component.For<IViewModelFactory, ViewModelFactory>()
                                        .LifeStyle.Transient,
                               Component.For<WorkspaceMapper>()
                                        .LifeStyle.Transient,
                               Component.For<IWorkspaceReaderWriter, BinaryFileWorkspaceReaderWriter>()
                                        .LifeStyle.Transient,
                               Component.For<IWorkspaceReader, BinaryFileWorkspaceReader>()
                                        .LifeStyle.Transient,
                               Component.For<IWorkspaceWriter, BinaryFileWorkspaceWriter>()
                                        .LifeStyle.Transient);

            return container;
        }
    }
}
