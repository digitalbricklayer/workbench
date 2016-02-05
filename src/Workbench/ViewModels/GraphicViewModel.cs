using System;
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
        /// Initialize a graphic with a data service.
        /// </summary>
        /// <param name="theGraphicModel">Graphic model.</param>
        protected GraphicViewModel(GraphicModel theGraphicModel)
        {
            if (theGraphicModel == null)
                throw new ArgumentNullException("theGraphicModel");
            this.Model = theGraphicModel;
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
            get { return this.Model.Name; }
            set
            {
                if (this.Model.Name == value) return;
                var oldVariableName = this.Model.Name;
                this.Model.Name = value;
                NotifyOfPropertyChange();
                this.OnRename(oldVariableName);
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
                return this.Model.X;
            }
            set
            {
                if (this.Model.X.Equals(value)) return;
                this.Model.X = value;
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
                return this.Model.Y;
            }
            set
            {
                if (this.Model.Y.Equals(value)) return;
                this.Model.Y = value;
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
                return this.Model.Id;
            }
        }

        /// <summary>
        /// Gets the edit domain name command.
        /// </summary>
        public ICommand EditNameCommand
        {
            get
            {
                return new CommandHandler(() => this.IsNameEditing = true);
            }
        }

        /// <summary>
        /// Hook called prior to a graphic is renamed.
        /// </summary>
        protected virtual void OnRename(string theOldName)
        {
            // Intentionally left blank, override as necessary.
        }
    }
}
