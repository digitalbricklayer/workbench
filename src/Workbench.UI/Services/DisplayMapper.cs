using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
#if true
    /// <summary>
    /// Map the display model to a solution designer view model.
    /// </summary>
    public class DisplayMapper
    {
        private readonly VariableMapper variableMapper;
        private readonly DomainMapper domainMapper;
        private readonly ConstraintMapper constraintMapper;
        private readonly IViewModelFactory viewModelFactory;
        private readonly IViewModelService viewModelService;
        private readonly IDataService dataService;

        public DisplayMapper(VariableMapper theVariableMapper,
                             ConstraintMapper theConstraintMapper,
                             DomainMapper theDomainMapper,
                             IViewModelFactory theViewModelFactory,
                             IViewModelService theViewModelService,
                             IDataService theDataService)
        {
            Contract.Requires<ArgumentNullException>(theVariableMapper != null);
            Contract.Requires<ArgumentNullException>(theConstraintMapper != null);
            Contract.Requires<ArgumentNullException>(theDomainMapper != null);
            Contract.Requires<ArgumentNullException>(theViewModelFactory != null);
            Contract.Requires<ArgumentNullException>(theViewModelService != null);

            this.viewModelService = theViewModelService;
            this.variableMapper = theVariableMapper;
            this.domainMapper = theDomainMapper;
            this.constraintMapper = theConstraintMapper;
            this.viewModelFactory = theViewModelFactory;
            this.dataService = theDataService;
        }

        /// <summary>
        /// Map a display model to a solution designer view model.
        /// </summary>
        /// <param name="theWorkspace">The workspace.</param>
        /// <returns>Workspace editor view model.</returns>
        public ModelEditorTabViewModel MapFrom(WorkspaceModel theWorkspace)
        {
            Contract.Requires<ArgumentNullException>(theWorkspace != null);

            var theModel = theWorkspace.Model;
            var newDesignerViewModel = new ModelEditorTabViewModel(this.dataService);

            foreach (var domainModel in theWorkspace.Model.Domains)
            {
                var domainViewModel = this.domainMapper.MapFrom(domainModel);
                newDesignerViewModel.FixupDomain(domainViewModel);
            }

            foreach (var constraintModel in theWorkspace.Model.Constraints)
            {
                var constraintViewModel = this.constraintMapper.MapFrom(constraintModel);
                newDesignerViewModel.FixupConstraint(constraintViewModel);
            }

            foreach (var variableModel in theWorkspace.Model.Singletons)
            {
                var variableViewModel = this.variableMapper.MapFrom(variableModel);
                newDesignerViewModel.FixupSingletonVariable(variableViewModel);
            }

            foreach (var aggregateModel in theWorkspace.Model.Aggregates)
            {
                var variableViewModel = this.variableMapper.MapFrom(aggregateModel);
                newDesignerViewModel.FixupAggregateVariable(variableViewModel);
            }

            return newDesignerViewModel;
        }
    }
#endif
}
