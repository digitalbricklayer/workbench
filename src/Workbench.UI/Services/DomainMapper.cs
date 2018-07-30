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
        private IEventAggregator eventAggregator;
        private IDataService dataService;

        public DomainMapper()
        {
        }

        public DomainModelItemViewModel MapFrom(DomainModel theDomainModel)
        {
            Contract.Requires<ArgumentNullException>(theDomainModel != null);
            Contract.Assert(theDomainModel.HasIdentity);

#if false
            var domainViewModel = new DomainModelItemViewModel(theDomainModel,
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