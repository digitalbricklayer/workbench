using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Base for all graphic elements displayed on the model view.
    /// </summary>
    public abstract class GraphicViewModel : Screen
    {
        /// <summary>
        /// Set to 'true' when the graphic is selected.
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// Is the name currently being edited.
        /// </summary>
        private bool isNameEditing;

        /// <summary>
        /// Initialize a graphic view model with a graphic model.
        /// </summary>
        /// <param name="theGraphicModel">Graphic model.</param>
        protected GraphicViewModel(GraphicModel theGraphicModel)
        {
            Contract.Requires<ArgumentNullException>(theGraphicModel != null);
            Model = theGraphicModel;
        }

        /// <summary>
        /// Gets or sets the graphic model.
        /// </summary>
        public GraphicModel Model { get; set; }

        /// <summary>
        /// Gets or sets the graphic name.
        /// </summary>
        public virtual string Name
        {
            get { return Model.Name; }
            set
            {
                var oldVariableName = this.Model.Name;
                Model.Name = value;
                NotifyOfPropertyChange();
                OnRename(oldVariableName);
            }
        }

        /// <summary>
        /// Gets or sets whether the graphic name is being edited.
        /// </summary>
        public bool IsNameEditing
        {
            get { return this.isNameEditing; }
            set
            {
                this.isNameEditing = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// The X coordinate for the position of the domain.
        /// </summary>
        public double X
        {
            get
            {
                return Model.X;
            }
            set
            {
                if (Model.X.Equals(value)) return;
                Model.X = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// The Y coordinate for the position of the domain.
        /// </summary>
        public double Y
        {
            get
            {
                return Model.Y;
            }
            set
            {
                if (Model.Y.Equals(value)) return;
                Model.Y = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets selected status of the domain.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                if (this.isSelected == value) return;
                // Selection state is not tracked by the model.
                this.isSelected = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the graphic identity.
        /// </summary>
        public int Id
        {
            get
            {
                return Model.Id;
            }
        }

        /// <summary>
        /// Gets the edit domain name command.
        /// </summary>
        public ICommand EditNameCommand
        {
            get
            {
                return new CommandHandler(() => IsNameEditing = true);
            }
        }

        /// <summary>
        /// Hook called prior to a graphic is renamed.
        /// </summary>
        protected virtual void OnRename(string theOldName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theOldName));
            // Intentionally left blank, override as necessary.
        }
    }
}
