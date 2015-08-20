using System;
using System.Collections.Generic;

namespace DynaApp.Models
{
    /// <summary>
    /// The solution model.
    /// </summary>
    [Serializable]
    public class SolutionModel : ModelBase
    {
        public SolutionModel()
        {
            this.Values = new List<ValueModel>();
        }

        public List<ValueModel> Values { get; set; }
        public ModelModel Model { get; set; }

        public void AddValue(ValueModel theValue)
        {
            this.Values.Add(theValue);
        }
    }
}