using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Workbench.Services;

namespace Workbench.Bootstrapper
{
    /// <summary>
    /// Installer for the Data Access Layer (DAL).
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
                            Component.For<IWorkspaceReaderWriter, WorkspaceReaderWriter>()
                                     .LifeStyle.Transient,
                            Component.For<IWorkspaceReader, XmlWorkspaceReader>()
                                     .LifeStyle.Transient,
                            Component.For<IWorkspaceWriter, XmlWorkspaceWriter>()
                                     .LifeStyle.Transient);
        }
    }
}