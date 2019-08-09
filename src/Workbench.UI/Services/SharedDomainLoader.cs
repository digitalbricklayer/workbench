using System.Diagnostics;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Maps a domain model into a view model.
    /// </summary>
    public class SharedDomainLoader
    {
        private readonly IWindowManager _windowManager;

        public SharedDomainLoader(IWindowManager theWindowManager)
        {
            _windowManager = theWindowManager;
        }

        public SharedDomainModelItemViewModel MapFrom(SharedDomainModel theDomainModel)
        {
            Debug.Assert(theDomainModel.HasIdentity);

            var domainViewModel = new SharedDomainModelItemViewModel(theDomainModel, _windowManager);

            return domainViewModel;
        }
    }
}