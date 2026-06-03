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

        public BundleModelItemViewModel(BundleTemplateModel bundle, ModelEditorTabViewModel modelEditorTab)
            : base(bundle)
        {
            _modelEditorTab = modelEditorTab;
            Validator = new BundleModelItemValidator();
            Bundle = bundle;
            DisplayName = Bundle.Name;
        }

        /// <summary>
        /// Gets the bundle model associated with the model item.
        /// </summary>
        public BundleTemplateModel Bundle { get; }

        /// <summary>
        /// Edit the contents of the bundle.
        /// </summary>
        public override void Edit()
        {
            _modelEditorTab.EditBundle(Bundle);
        }
    }
}
