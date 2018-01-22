using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Workbench.Services;

namespace Workbench.Bootstrapper
{
    /// <summary>
    /// Register application infrastructure into the Windsor container.
    /// </summary>
    internal class InfrastructureInstaller : IRegistration
    {
        /// <summary>
        /// Performs the registration in the <see cref="T:Castle.MicroKernel.IKernel"/>.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public void Register(IKernelInternal kernel)
        {
            kernel.Register(Component.For<IAppRuntime, AppRuntime>()
                                     .LifeStyle.Singleton,
                            Component.For<IViewModelFactory, ViewModelFactory>()
                                     .LifeStyle.Transient,
                            Component.For<IViewModelService, ViewModelService>()
                                     .LifeStyle.Singleton,
                            Component.For<WorkAreaMapper>()
                                     .LifeStyle.Transient,
                            Component.For<SolutionMapper>()
                                     .LifeStyle.Transient,
                            Component.For<DisplayMapper>()
                                     .LifeStyle.Transient,
                            Component.For<VariableMapper>()
                                     .LifeStyle.Transient,
                            Component.For<ConstraintMapper>()
                                     .LifeStyle.Transient,
                            Component.For<DomainMapper>()
                                     .LifeStyle.Transient);
        }
    }
}
