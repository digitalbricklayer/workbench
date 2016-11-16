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
            kernel.Register(Component.For<AddChessboardVisualizerCommand>()
                                     .LifeStyle.Transient,
                            Component.For<EditSolutionCommand>()
                                     .LifeStyle.Transient,
                            Component.For<AddMapVisualizerCommand>()
                                     .LifeStyle.Transient,
                            Component.For<EditGridCommand>()
                                     .LifeStyle.Transient);
        }
    }
}
