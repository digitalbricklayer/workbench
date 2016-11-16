using System;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A column inside a grid.
    /// </summary>
    [Serializable]
    public class GridColumnModel : AbstractModel
    {
        private string name;

        /// <summary>
        /// Gets or sets the grid column name.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
    }
}