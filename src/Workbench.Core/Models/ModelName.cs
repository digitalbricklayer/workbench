using System;

namespace Workbench.Core.Models
{
    [Serializable]
    public sealed class ModelName : AbstractModel
    {
        private string _name;

        public ModelName(string theName)
        {
            Text = theName;
        }

        public ModelName()
        {
            Text = string.Empty;
        }

        public string Text
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
    }
}
