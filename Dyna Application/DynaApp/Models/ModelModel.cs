using System;
using System.Collections.Generic;

namespace DynaApp.Models
{
    /// <summary>
    /// The model model.
    /// </summary>
    [Serializable]
    public class ModelModel
    {
        public List<VariableModel> Variables { get; set; }
        public List<DomainModel> Domains { get; set; }
        public List<ConstraintModel> Constraints { get; set; }
        public List<ConnectionModel> Connections { get; set; }
    }
}