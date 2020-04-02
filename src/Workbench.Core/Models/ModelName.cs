using System;

namespace Workbench.Core.Models
{
    [Serializable]
    public sealed class ModelName : AbstractModel
    {
        private string _name;

        /// <summary>
        /// Initializes a new model name with a name.
        /// </summary>
        /// <param name="theName"></param>
        public ModelName(string theName)
        {
            Text = theName;
        }

        /// <summary>
        /// Initializes a new model name with default values.
        /// </summary>
        public ModelName()
        {
            Text = string.Empty;
        }

        public static implicit operator string(ModelName name) =>
            // Convert the model name into a string
            name.Text;

        public string Text
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Is the name equal to the model name?
        /// </summary>
        /// <param name="theName">Name string</param>
        /// <returns>True if equal, False if not equal.</returns>
        public bool IsEqualTo(string theName)
        {
            return Text == theName;
        }
    }
}
