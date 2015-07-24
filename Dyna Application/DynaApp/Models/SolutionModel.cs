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
        public List<ValueModel> Values { get; set; }
    }
}