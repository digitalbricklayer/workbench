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

        internal DomainEditorViewModel MapFrom(DomainGraphicModel theDomainModel)
        {
            Debug.Assert(theDomainModel.HasIdentity);

#if false
            var domainViewModel = new DomainEditorViewModel(theDomainModel);

            this.cache.CacheGraphic(domainViewModel);

            return domainViewModel;
#else
            return null;
#endif
        }
    }
}