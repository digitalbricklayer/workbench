namespace Workbench.Services
{
    /// <summary>
    /// Manage the single instance of the workspace document.
    /// </summary>
    public class DocumentManager : IDocumentManager
    {
        private IWorkspaceDocument _currentDocument;
        private readonly IViewModelFactory _viewModelFactory;

        /// <summary>
        /// Initialize a new document manager with a view model factory.
        /// </summary>
        /// <param name="theViewModelFactory">View model factory.</param>
        public DocumentManager(IViewModelFactory theViewModelFactory)
        {
            _viewModelFactory = theViewModelFactory;
        }

        /// <summary>
        /// Gets or sets the current document.
        /// </summary>
        public IWorkspaceDocument CurrentDocument
        {
            get => _currentDocument;
            set
            {
                _currentDocument = value;
            }
        }

        /// <summary>
        /// Create a new document.
        /// </summary>
        public IWorkspaceDocument CreateDocument()
        {
            var newDocument = _viewModelFactory.CreateDocument();
            newDocument.New();
            CurrentDocument = newDocument;

            return CurrentDocument;
        }
    }
}
