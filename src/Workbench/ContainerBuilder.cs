using Caliburn.Micro;
using Castle.MicroKernel;
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

            container.Register(new ViewModelInstaller(),
                               new CaliburnInfrastructureInstaller(),
                               new DalInstaller(),
                               Component.For<IAppRuntime, AppRuntime>()
                                        .LifeStyle.Singleton,
                               Component.For<IViewModelFactory, ViewModelFactory>()
                                        .LifeStyle.Transient,
                               Component.For<WorkspaceMapper>()
                                        .LifeStyle.Transient);

            return container;
        }
    }

    /// <summary>
    /// Installer for the Data access Layer.
    /// </summary>
    internal class DalInstaller : IRegistration
    {
        /// <summary>
        /// Performs the registration in the <see cref="T:Castle.MicroKernel.IKernel"/>.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public void Register(IKernelInternal kernel)
        {
            kernel.Register(Component.For<IDataService, DataService>()
                                     .LifeStyle.Singleton,
                            Component.For<IWorkspaceReaderWriter, BinaryFileWorkspaceReaderWriter>()
                                     .LifeStyle.Transient,
                            Component.For<IWorkspaceReader, BinaryFileWorkspaceReader>()
                                     .LifeStyle.Transient,
                            Component.For<IWorkspaceWriter, BinaryFileWorkspaceWriter>()
                                     .LifeStyle.Transient);
        }
    }

    /// <summary>
    /// Installer for the Caliburn Micro infrastructure.
    /// </summary>
    internal class CaliburnInfrastructureInstaller : IRegistration
    {
        /// <summary>
        /// Performs the registration in the <see cref="T:Castle.MicroKernel.IKernel"/>.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public void Register(IKernelInternal kernel)
        {
            kernel.Register(Component.For<IWindowManager, WindowManager>()
                                     .LifeStyle.Singleton,
                            Component.For<IEventAggregator, EventAggregator>()
                                     .LifeStyle.Singleton);
        }
    }

    /// <summary>
    /// Installer for view models.
    /// </summary>
    public class ViewModelInstaller : IRegistration
    {
        /// <summary>
        /// Performs the registration in the <see cref="T:Castle.MicroKernel.IKernel"/>.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public void Register(IKernelInternal kernel)
        {
            kernel.Register(Component.For<IShell, ShellViewModel>()
                                     .LifeStyle.Singleton,
                            Component.For<WorkspaceViewModel>()
                                     .LifeStyle.Singleton,
                            Component.For<ApplicationMenuViewModel>()
                                     .LifeStyle.Singleton,
                            Component.For<TitleBarViewModel>()
                                     .LifeStyle.Singleton);
        }
    }
}
