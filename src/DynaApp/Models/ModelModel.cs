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

        public ModelModel()
        {
            this.Variables = new List<VariableModel>();
            this.Domains = new List<DomainModel>();
            this.Constraints = new List<ConstraintModel>();
            this.Connections = new List<ConnectionModel>();
        }

        public void AddConstraint(ConstraintModel constraint)
        {
            this.Constraints.Add(constraint);
        }

        public void AddVariable(VariableModel variableModel)
        {
            this.Variables.Add(variableModel);
        }

        public void AddDomain(DomainModel domain)
        {
            this.Domains.Add(domain);
        }

        public void Connect(VariableModel variableModel, GraphicModel endPoint)
        {
        }
    }
}