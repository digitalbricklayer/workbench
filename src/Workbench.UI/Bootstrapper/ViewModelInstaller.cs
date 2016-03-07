using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Workbench.ViewModels;

namespace Workbench.Bootstrapper
{
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