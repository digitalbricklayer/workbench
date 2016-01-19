using Caliburn.Micro;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;

namespace Workbench.Bootstrapper
{
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
}