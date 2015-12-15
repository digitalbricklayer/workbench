using System.Diagnostics;
using Dyna.Core.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    /// <summary>
    /// Maps a domain model into a view model.
    /// </summary>
    internal class DomainMapper
    {
        private readonly ModelViewModelCache cache;

        internal DomainMapper(ModelViewModelCache theCache)
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