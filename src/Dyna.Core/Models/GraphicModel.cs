using System;

namespace Dyna.Core.Models
{
    [Serializable]
    public abstract class GraphicModel : ModelBase
    {
        /// <summary>
        /// Initialize a graphic model with a name.
        /// </summary>
        /// <param name="connectableName">Connectable name.</param>
        protected GraphicModel(string connectableName)
            : this()
        {
            this.Name = connectableName;
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
