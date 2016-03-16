using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Workbench.Commands;

namespace Workbench.Bootstrapper
{
    /// <summary>
    /// Install all commands into the container.
    /// </summary>
    class CommandInstaller : IRegistration
    {
        /// <summary>
        /// Performs the registration in the <see cref="T:Castle.MicroKernel.IKernel"/>.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public void Register(IKernelInternal kernel)
        {
            kernel.Register(Component.For<AddVariableVisualizerCommand>()
                                     .LifeStyle.Singleton,
                            Component.For<AddChessboardVisualizerCommand>()
                                     .LifeStyle.Singleton);
        }
    }
}
