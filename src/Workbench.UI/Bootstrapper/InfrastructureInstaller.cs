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
            kernel.Register(Component.For<IViewModelFactory, ViewModelFactory>().LifeStyle.Transient,
                            Component.For<ModelEditorLoader>().LifeStyle.Transient,
                            Component.For<SharedDomainLoader>().LifeStyle.Transient,
                            Component.For<ConstraintLoader>().LifeStyle.Transient,
                            Component.For<VariableLoader>().LifeStyle.Transient,
                            Component.For<IWorkspaceLoader, WorkspaceLoader>().LifeStyle.Transient,
                            Component.For<IResourceManager, ResourceManager>().LifeStyle.Singleton);
        }
    }
}
