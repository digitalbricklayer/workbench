using Caliburn.Micro;

namespace Workbench.ViewModels
{
    public class GridEditorViewModel : Screen
    {
        private string backgroundImagePath;

        /// <summary>
        /// Gets or sets the background image path.
        /// </summary>
        public string BackgroundImagePath
        {
            get { return this.backgroundImagePath; }
            set
            {
                this.backgroundImagePath = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Okay button clicked.
        /// </summary>
        public void AcceptButton()
        {
            TryClose(true);
        }
    }
}
