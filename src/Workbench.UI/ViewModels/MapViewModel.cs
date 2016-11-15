using System;
using System.Diagnostics.Contracts;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class MapViewModel : Screen
    {
        private readonly MapModel model;
        private Image backgroundImage;

        public MapViewModel(MapModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            this.model = theModel;
            this.backgroundImage = new Image();
            if (this.model.HasBackgroundImage)
            {
                LoadCustomBackgroundImage();
            }
            else
            {
                LoadDefaultBackgroundImage();
            }
        }

        /// <summary>
        /// Gets the map model.
        /// </summary>
        public MapModel Map
        {
            get { return this.model; }
        }

        /// <summary>
        /// Gets or sets the map background image.
        /// </summary>
        public Image BackgroundImage
        {
            get { return this.backgroundImage; }
            set
            {
                this.backgroundImage = value;
                NotifyOfPropertyChange();
            }
        }

        private void LoadCustomBackgroundImage()
        {
            Contract.Ensures(this.model.HasBackgroundImage);
            var src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(this.model.BackgroundImagePath, UriKind.Absolute);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            BackgroundImage.Source = src;
            BackgroundImage.Stretch = Stretch.Uniform;
        }

        private void LoadDefaultBackgroundImage()
        {
            Contract.Ensures(!this.model.HasBackgroundImage);
            var src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri("../../../Images/Transparent.png", UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            BackgroundImage.Source = src;
            BackgroundImage.Stretch = Stretch.Uniform;
        }
    }
}
