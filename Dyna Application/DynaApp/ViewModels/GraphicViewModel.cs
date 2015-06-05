using System;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// Base for all graphic elements displayed on the model view.
    /// </summary>
    public abstract class GraphicViewModel : AbstractModelBase
    {
        /// <summary>
        /// The X coordinate for the position of the domain.
        /// </summary>
        private double x;

        /// <summary>
        /// The Y coordinate for the position of the domain.
        /// </summary>
        private double y;

        /// <summary>
        /// Set to 'true' when the domain is selected.
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// The domain name.
        /// </summary>
        private string name;

        /// <summary>
        /// Initialize a graphic with a name.
        /// </summary>
        /// <param name="newName"></param>
        protected GraphicViewModel(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("newName");
            this.Name = newName;
        }

        /// <summary>
        /// Initialize a graphc with default values.
        /// </summary>
        protected GraphicViewModel()
        {
            this.Name = string.Empty;
        }

        /// <summary>
        /// Gets or sets the domain name.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                if (this.name == value) return;
                this.name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// The X coordinate for the position of the domain.
        /// </summary>
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                if (x == value) return;
                x = value;
                OnPropertyChanged("X");
            }
        }

        /// <summary>
        /// The Y coordinate for the position of the domain.
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                if (y == value) return;
                y = value;
                OnPropertyChanged("Y");
            }
        }

        /// <summary>
        /// Gets or sets selected status of the domain.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (isSelected == value) return;
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
    }
}
