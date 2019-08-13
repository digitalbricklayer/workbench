using System;
using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Document path.
    /// </summary>
    public class DocumentPathViewModel : Screen 
    {
        private string _path;

        /// <summary>
        /// Initialize the document path with the document path.
        /// </summary>
        public DocumentPathViewModel(string thePath)
        {
            if (string.IsNullOrWhiteSpace(thePath))
                throw new ArgumentException("Path must not be blank", nameof(thePath));

            FullPath = thePath;
        }

        /// <summary>
        /// Initialize the document path with default values.
        /// </summary>
        public DocumentPathViewModel()
        {
            FullPath = string.Empty;
        }

        /// <summary>
        /// Gets or sets the path to the document.
        /// </summary>
        public string FullPath
        {
            get => _path;
            set
            {
                _path = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets whether the path has been set.
        /// </summary>
        public bool IsEmpty => string.IsNullOrWhiteSpace(FullPath);
    }
}
