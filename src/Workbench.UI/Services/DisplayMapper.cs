using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Map the display model to a solution designer view model.
    /// </summary>
    public class DisplayMapper
    {
        private readonly VariableMapper variableMapper;
        private readonly DomainMapper domainMapper;
        private readonly ConstraintMapper constraintMapper;
        private readonly IViewModelFactory viewModelFactory;
        private readonly IViewModelService _viewModelService;

        public DisplayMapper(VariableMapper theVariableMapper,
                             ConstraintMapper theConstraintMapper,
                             DomainMapper theDomainMapper,
                             IViewModelFactory theViewModelFactory,
                             IViewModelService theViewModelService)
        {
            Contract.Requires<ArgumentNullException>(theVariableMapper != null);
            Contract.Requires<ArgumentNullException>(theConstraintMapper != null);
            Contract.Requires<ArgumentNullException>(theDomainMapper != null);
            Contract.Requires<ArgumentNullException>(theViewModelFactory != null);
            Contract.Requires<ArgumentNullException>(theViewModelService != null);

            this._viewModelService = theViewModelService;
            this.variableMapper = theVariableMapper;
            this.domainMapper = theDomainMapper;
            this.constraintMapper = theConstraintMapper;
            this.viewModelFactory = theViewModelFactory;
        }

        /// <summary>
        /// Map a display model to a solution designer view model.
        /// </summary>
        /// <param name="theDisplay">Display model.</param>
        /// <returns>Solution designer view model.</returns>
        public WorkspaceEditorViewModel MapFrom(WorkspaceModel theWorkspace)
        {
            Contract.Requires<ArgumentNullException>(theWorkspace != null);

            var theModel = theWorkspace.Model;
            var newDesignerViewModel = new WorkspaceEditorViewModel(theWorkspace.Solution.Display, theModel);

            foreach (var domainModel in theWorkspace.Model.Domains)
            {
#if false
                var domainViewModel = this.domainMapper.MapFrom(domainModel);
                newDesignerViewModel.FixupDomain(domainViewModel);
#endif
            }

            foreach (var constraintModel in theWorkspace.Model.Constraints)
            {
#if false
                var constraintViewModel = this.constraintMapper.MapFrom(constraintModel);
                newDesignerViewModel.FixupConstraint(constraintViewModel);
#endif
            }

            foreach (var variableModel in theWorkspace.Model.Singletons)
            {
#if false
                var variableViewModel = this.variableMapper.MapFrom(variableModel);
                newDesignerViewModel.FixupSingletonVariable(variableViewModel);
#endif
            }

            foreach (var aggregateModel in theWorkspace.Model.Aggregates)
            {
#if false
                var variableViewModel = this.variableMapper.MapFrom(aggregateModel);
                newDesignerViewModel.FixupAggregateVariable(variableViewModel);
#endif
            }

            return newDesignerViewModel;
        }
    }
}
