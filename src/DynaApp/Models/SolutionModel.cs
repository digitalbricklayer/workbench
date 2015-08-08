using System;
using System.Collections.Generic;

namespace DynaApp.Models
{
    /// <summary>
    /// The solution model.
    /// </summary>
    [Serializable]
    public class SolutionModel
    {
        public SolutionModel()
        {
            this.Values = new List<ValueModel>();
        }

        public List<ValueModel> Values { get; set; }

        public void AddValue(ValueModel theValue)
        {
            this.Values.Add(theValue);
        }
    }
}