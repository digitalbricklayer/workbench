using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public abstract class VisualizerViewModel : Screen
    {
        private EditorViewModel editor;
        private ViewerViewModel viewer;
        private readonly Model model;

        protected VisualizerViewModel(Model theModel, EditorViewModel theEditor, ViewerViewModel theViewer)
        {
            Contract.Requires<ArgumentNullException>(theEditor != null);
            Contract.Requires<ArgumentNullException>(theViewer != null);

            this.model = theModel;
            Editor = theEditor;
            Viewer = theViewer;
        }

        /// <summary>
        /// Gets the variable model.
        /// </summary>
        public Model Model
        {
            get { return this.model; }
        }

        /// <summary>
        /// Gets the variable name.
        /// </summary>
        public string Name
        {
            get { return Model.Name.Text; }
        }

        /// <summary>
        /// Gets or sets the editor.
        /// </summary>
        public EditorViewModel Editor
        {
            get { return this.editor; }
            set
            {
                this.editor = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the viewer.
        /// </summary>
        public ViewerViewModel Viewer
        {
            get { return this.viewer; }
            set
            {
                this.viewer = value;
                NotifyOfPropertyChange();
            }
        }

        public int Id
        {
            get
            {
                return Editor.Model.Id;
            }
        }
    }
}
