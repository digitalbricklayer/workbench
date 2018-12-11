using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Load the model editor view model synchronized with the model.
    /// </summary>
    public sealed class ModelEditorLoader
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
        /// Load a model editor from the workspace model.
        /// </summary>
        /// <param name="theWorkspace">Workspace model.</param>
        /// <returns>Model editor tab view model.</returns>
        public ModelEditorTabViewModel LoadFrom(WorkspaceModel theWorkspace)
        {
            Contract.Requires<ArgumentNullException>(theWorkspace != null);

            var newModelEditor = _viewModelFactory.CreateModelEditor();

            LoadSharedDomains(theWorkspace, newModelEditor);
            LoadConstraints(theWorkspace, newModelEditor);
            LoadSingletonVariables(theWorkspace, newModelEditor);
            LoadAggregateVariables(theWorkspace, newModelEditor);

            return newModelEditor;
        }

        private void LoadAggregateVariables(WorkspaceModel theWorkspace, ModelEditorTabViewModel newModelEditor)
        {
            foreach (var aggregateModel in theWorkspace.Model.Aggregates)
            {
                var variableViewModel = _variableLoader.MapFrom(aggregateModel);
                newModelEditor.FixupAggregateVariable(variableViewModel);
            }
        }

        private void LoadSingletonVariables(WorkspaceModel theWorkspace, ModelEditorTabViewModel newModelEditor)
        {
            foreach (var variableModel in theWorkspace.Model.Singletons)
            {
                var variableViewModel = _variableLoader.MapFrom(variableModel);
                newModelEditor.FixupSingletonVariable(variableViewModel);
            }
        }

        private void LoadConstraints(WorkspaceModel theWorkspace, ModelEditorTabViewModel newModelEditor)
        {
            foreach (var constraintModel in theWorkspace.Model.Constraints)
            {
                var constraintViewModel = _constraintLoader.MapFrom(constraintModel);
                newModelEditor.FixupConstraint(constraintViewModel);
            }
        }

        private void LoadSharedDomains(WorkspaceModel theWorkspace, ModelEditorTabViewModel newDesignerViewModel)
        {
            foreach (var domainModel in theWorkspace.Model.SharedDomains)
            {
                var domainViewModel = _sharedDomainLoader.MapFrom(domainModel);
                newDesignerViewModel.FixupDomain(domainViewModel);
            }
        }
    }
}
