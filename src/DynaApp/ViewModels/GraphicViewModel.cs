using System;
using System.Windows;
using System.Windows.Input;
using DynaApp.Models;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// Base for all graphic elements displayed on the model view.
    /// </summary>
    public abstract class GraphicViewModel : AbstractViewModel
    {
        /// <summary>
        /// The X coordinate for the position of the graphic.
        /// </summary>
        private double x;

        /// <summary>
        /// The Y coordinate for the position of the graphic.
        /// </summary>
        private double y;

        /// <summary>
        /// Set to 'true' when the graphic is selected.
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// The graphic name.
        /// </summary>
        private string name;

        /// <summary>
        /// Is the name currently being edited.
        /// </summary>
        private bool isNameEditing;

        /// <summary>
        /// Initialize a graphic with a name and location.
        /// </summary>
        protected GraphicViewModel(string newName, Point newLocation)
            : this(newName)
        {
            this.X = newLocation.X;
            this.Y = newLocation.Y;
        }

        /// <summary>
        /// Initialize a graphic with a name.
        /// </summary>
        /// <param name="newName"></param>
        protected GraphicViewModel(string newName)
            : this()
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("newName");
            this.Name = newName;
        }

        /// <summary>
        /// Initialize a graphic with default values.
        /// </summary>
        protected GraphicViewModel()
        {
            this.Name = string.Empty;
        }

        /// <summary>
        /// Gets or sets the graphic model.
        /// </summary>
        public GraphicModel Model { get; set; }

        /// <summary>
        /// Gets or sets the graphic name.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                if (this.name == value) return;
                this.name = value;
                this.UpdateModelName(value);
                OnPropertyChanged();
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
                if (this.isNameEditing == value) return;
                this.isNameEditing = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The X coordinate for the position of the domain.
        /// </summary>
        public double X
        {
            get
            {
                return this.x;
            }
            set
            {
                if (this.x.Equals(value)) return;
                this.x = value;
                this.UpdateModelX(value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The Y coordinate for the position of the domain.
        /// </summary>
        public double Y
        {
            get
            {
                return this.y;
            }
            set
            {
                if (this.y.Equals(value)) return;
                this.y = value;
                this.UpdateModelY(value);
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the graphic identity.
        /// </summary>
        public int Id
        {
            get { return this.Model.Id; }
        }

        /// <summary>
        /// Gets the edit domain name command.
        /// </summary>
        public ICommand EditNameCommand
        {
            get
            {
                return new CommandHandler(() => this.IsNameEditing = true, true);
            }
        }

        /// <summary>
        /// Update the graphic name.
        /// </summary>
        /// <param name="newName">New name.</param>
        private void UpdateModelName(string newName)
        {
            if (this.Model == null) return;
            this.Model.Name = newName;
        }

        private void UpdateModelX(double newValue)
        {
            if (this.Model == null) return;
            this.Model.X = newValue;
        }

        private void UpdateModelY(double newValue)
        {
            if (this.Model == null) return;
            this.Model.Y = newValue;
        }
    }
}
