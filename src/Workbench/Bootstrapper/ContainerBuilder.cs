using Castle.Windsor;

namespace Workbench.Bootstrapper
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
                               new InfrastructureInstaller());

            return container;
        }
    }
}
