using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using Workbench.Validators;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Bundle model item.
    /// </summary>
    public sealed class BundleModelItemViewModel : ModelItemViewModel
    {
        private readonly ModelEditorTabViewModel _modelEditorTab;

        public BundleModelItemViewModel(BundleModel bundle, ModelEditorTabViewModel modelEditorTab)
            : base(bundle)
        {
            Contract.Requires<ArgumentNullException>(modelEditorTab != null);
            _modelEditorTab = modelEditorTab;
            Validator = new BundleModelItemValidator();
            Bundle = bundle;
            DisplayName = Bundle.Name;
        }

        /// <summary>
        /// Gets the bundle model associated with the model item.
        /// </summary>
        public BundleModel Bundle { get; }

        /// <summary>
        /// Edit the contents of the bundle.
        /// </summary>
        public override void Edit()
        {
            _modelEditorTab.EditBundle(Bundle);
        }
    }
}
