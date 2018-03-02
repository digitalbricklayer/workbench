using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    [Serializable]
    public sealed class ModelName : AbstractModel
    {
        private string _name;

        public ModelName(string theName)
        {
            Contract.Requires<ArgumentNullException>(theName != null);
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
                Contract.Requires<ArgumentNullException>(value != null);
                _name = value;
                OnPropertyChanged();
            }
        }
    }
}
