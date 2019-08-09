using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench.Commands
{
    /// <summary>
    /// Command for the Solution|Edit Solution menu item.
    /// </summary>
    public class EditSolutionCommand : CommandBase
    {
        private readonly IWindowManager _windowManager;
        private IWorkspace _workspace;
        private readonly IDocumentManager _documentManager;

        public EditSolutionCommand(IWindowManager theWindowManager, IDocumentManager theDocumentManager)
        {
            _windowManager = theWindowManager;
            _documentManager = theDocumentManager;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        public override void Execute(object parameter)
        {
            _workspace = _documentManager.CurrentDocument.Workspace;
            var visualizerExpressionItems = CreateVisualizerCollectionFrom(_workspace.Bindings);
            var solutionEditorViewModel = new SolutionEditorViewModel(visualizerExpressionItems, _windowManager);
            var showDialogResult = _windowManager.ShowDialog(solutionEditorViewModel);
            if (!showDialogResult.GetValueOrDefault()) return;
			UpdateBindingsFrom(solutionEditorViewModel);
        }

        /// <summary>
        /// Update visualizer bindings from the visualizer expression editor view models.
        /// </summary>
        /// <param name="solutionEditor">Binding expression editors.</param>
        private void UpdateBindingsFrom(SolutionEditorViewModel solutionEditor)
        {
            foreach (var visualizerEditorId in solutionEditor.Deleted)
            {
                var anUpdatedVisualizerBinding = _workspace.GetBindingExpressionById(visualizerEditorId);
                _workspace.DeleteBindingExpression(anUpdatedVisualizerBinding);
            }

            foreach (var updatedExpressionEditor in solutionEditor.Updated)
            {
                // Update existing expression
                var anUpdatedVisualizerBinding = _workspace.GetBindingExpressionById(updatedExpressionEditor.Id);
                anUpdatedVisualizerBinding.Text = updatedExpressionEditor.Text;
            }

            foreach (var newExpressionEditor in solutionEditor.Added)
            {
                // Add new expression
                var aNewExpression = new VisualizerBindingExpressionModel(newExpressionEditor.Text);
                _workspace.AddBindingExpression(aNewExpression);
            }
        }

        /// <summary>
        /// Create binding visualizer editor view models from the expression view models.
        /// </summary>
        /// <param name="bindings">Visualizer expression view models.</param>
        /// <returns>View model editors for the expressions.</returns>
        private ObservableCollection<VisualizerExpressionItemViewModel> CreateVisualizerCollectionFrom(IEnumerable<VisualizerBindingExpressionViewModel> bindings)
        {
            var visualizerExpressions = new ObservableCollection<VisualizerExpressionItemViewModel>();
            foreach (var binding in bindings)
            {
                visualizerExpressions.Add(new VisualizerExpressionItemViewModel(binding.Id, binding.Text));
            }

            return visualizerExpressions;
        }
    }
}
