using System.Diagnostics;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Maps a domain model into a view model.
    /// </summary>
    internal class DomainMapper
    {
        private readonly ViewModelCache cache;

        internal DomainMapper(ViewModelCache theCache)
        {
            this.cache = theCache;
        }

        internal DomainViewModel MapFrom(DomainModel theDomainModel)
        {
            Debug.Assert(theDomainModel.HasIdentity);

            var domainViewModel = new DomainViewModel(theDomainModel);

            this.cache.CacheGraphic(domainViewModel);

            return domainViewModel;
        }
    }
}