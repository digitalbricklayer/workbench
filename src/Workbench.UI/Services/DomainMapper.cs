using System.Diagnostics;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Maps a domain model into a view model.
    /// </summary>
    public class DomainMapper
    {
        private readonly IViewModelService cache;

        public DomainMapper(IViewModelService theService)
        {
            this.cache = theService;
        }

        internal DomainViewModel MapFrom(DomainGraphicModel theDomainModel)
        {
            Debug.Assert(theDomainModel.HasIdentity);

            var domainViewModel = new DomainViewModel(theDomainModel);

            this.cache.CacheGraphic(domainViewModel);

            return domainViewModel;
        }
    }
}