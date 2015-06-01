using System;

namespace DynaApp.Entities
{
    /// <summary>
    /// Fluent interface for building models.
    /// </summary>
    class ModelContext
    {
        private readonly Model model;

        public ModelContext(Model theModel)
        {
            if (theModel == null)
                throw new ArgumentNullException("theModel");
            this.model = theModel;
        }

        public ModelContext AddVariable(string theVariableName)
        {
            this.model.AddVariable(new Variable(theVariableName));
            return this;
        }

        public ModelContext WithDomain(string theDomainExpression)
        {
            var newDomain = new Domain(theDomainExpression);
            this.model.AddDomain(newDomain);
            foreach (var variable in this.model.Variables)
            {
                variable.Domain = newDomain;
            }
            return this;
        }

        public ModelContext WithDomain(Domain theDomainExpression)
        {
            this.model.AddDomain(theDomainExpression);
            foreach (var variable in this.model.Variables)
            {
                variable.Domain = theDomainExpression;
            }
            return this;
        }

        public ModelContext WithConstraint(string theConstraintExpression)
        {
            this.model.AddConstraint(new Constraint(theConstraintExpression));
            return this;
        }

        public Model Build()
        {
            return this.model;
        }
    }
}