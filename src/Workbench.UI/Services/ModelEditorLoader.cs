using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Load the model editor view model synchronized with the model.
    /// </summary>
    public class ModelEditorLoader
    {
        private readonly VariableLoader _variableLoader;
        private readonly SharedDomainLoader _sharedDomainLoader;
        private readonly ConstraintLoader _constraintLoader;
        private readonly IViewModelFactory _viewModelFactory;

        public ModelEditorLoader(VariableLoader theVariableLoader,
                                 ConstraintLoader theConstraintLoader,
                                 SharedDomainLoader theSharedDomainLoader,
                                 IViewModelFactory theViewModelFactory)
        {
            Contract.Requires<ArgumentNullException>(theVariableLoader != null);
            Contract.Requires<ArgumentNullException>(theConstraintLoader != null);
            Contract.Requires<ArgumentNullException>(theSharedDomainLoader != null);
            Contract.Requires<ArgumentNullException>(theViewModelFactory != null);

            _variableLoader = theVariableLoader;
            _sharedDomainLoader = theSharedDomainLoader;
            _constraintLoader = theConstraintLoader;
            _viewModelFactory = theViewModelFactory;
        }

        /// <summary>
        /// Map a display model to a solution designer view model.
        /// </summary>
        /// <param name="theWorkspace">Workspace model.</param>
        /// <returns>Model editor tab view model.</returns>
        public ModelEditorTabViewModel MapFrom(WorkspaceModel theWorkspace)
        {
            Contract.Requires<ArgumentNullException>(theWorkspace != null);

            var newDesignerViewModel = _viewModelFactory.CreateModelEditor();

            foreach (var domainModel in theWorkspace.Model.SharedDomains)
            {
                var domainViewModel = _sharedDomainLoader.MapFrom(domainModel);
                newDesignerViewModel.FixupDomain(domainViewModel);
            }

            foreach (var constraintModel in theWorkspace.Model.Constraints)
            {
                var constraintViewModel = _constraintLoader.MapFrom(constraintModel);
                newDesignerViewModel.FixupConstraint(constraintViewModel);
            }

            foreach (var variableModel in theWorkspace.Model.Singletons)
            {
                var variableViewModel = _variableLoader.MapFrom(variableModel);
                newDesignerViewModel.FixupSingletonVariable(variableViewModel);
            }

            foreach (var aggregateModel in theWorkspace.Model.Aggregates)
            {
                var variableViewModel = _variableLoader.MapFrom(aggregateModel);
                newDesignerViewModel.FixupAggregateVariable(variableViewModel);
            }

            return newDesignerViewModel;
        }
    }
}