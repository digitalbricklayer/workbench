using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
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
        private IEventAggregator eventAggregator;
        private IDataService dataService;

        public DomainMapper(IViewModelService theService)
        {
            Contract.Requires<ArgumentNullException>(theService != null);
            this.cache = theService;
        }

        public DomainEditorViewModel MapFrom(DomainGraphicModel theDomainModel)
        {
            Contract.Requires<ArgumentNullException>(theDomainModel != null);
            Contract.Assert(theDomainModel.HasIdentity);

#if false
            var domainViewModel = new DomainEditorViewModel(theDomainModel,
                                                            this.eventAggregator,
                                                            this.dataService,
                                                            this.cache);

            this.cache.CacheGraphic(domainViewModel);

            return domainViewModel;
#else
            return null;
#endif
        }
    }
}