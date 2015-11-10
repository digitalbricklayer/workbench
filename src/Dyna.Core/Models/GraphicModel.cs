using System;
using System.Windows;

namespace Dyna.Core.Models
{
    [Serializable]
    public abstract class GraphicModel : ModelBase
    {
        /// <summary>
        /// Initialize a graphic model with a name and location.
        /// </summary>
        /// <param name="graphicName">Name for the graphic.</param>
        /// <param name="location">Location of the graphic.</param>
        protected GraphicModel(string graphicName, Point location)
            : this(graphicName)
        {
            this.X = location.X;
            this.Y = location.Y;
        }

        /// <summary>
        /// Initialize a graphic model with a name.
        /// </summary>
        /// <param name="graphicName">Name for the graphic.</param>
        protected GraphicModel(string graphicName)
            : this()
        {
            this.Name = graphicName;
        }

        /// <summary>
        /// Initialize a graphic model with default values.
        /// </summary>
        protected GraphicModel()
        {
            this.Name = string.Empty;
        }

        /// <summary>
        /// Gets or sets the graphic name.
        /// </summary>
        public string Name { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
    }
}
