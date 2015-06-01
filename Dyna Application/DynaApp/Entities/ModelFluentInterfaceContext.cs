using System;

namespace DynaApp.Entities
{
    /// <summary>
    /// Fluent interface for building models.
    /// </summary>
    class ModelFluentInterfaceContext
    {
        private readonly Model model;

        public ModelFluentInterfaceContext(Model theModel)
        {
            if (theModel == null)
                throw new ArgumentNullException("theModel");
            this.model = theModel;
        }

        public ModelFluentInterfaceContext AddVariable(string theVariableName)
        {
            this.model.AddVariable(new Variable(theVariableName));
            return this;
        }

        public ModelFluentInterfaceContext WithDomain(string theDomainExpression)
        {
            var x = new Domain(theDomainExpression);
            this.model.AddDomain(x);
            foreach (var variable in this.model.Variables)
            {
                variable.Domain = x;
            }
            return this;
        }

        public ModelFluentInterfaceContext WithDomain(Domain theDomainExpression)
        {
            this.model.AddDomain(theDomainExpression);
            foreach (var variable in this.model.Variables)
            {
                variable.Domain = theDomainExpression;
            }
            return this;
        }

        public ModelFluentInterfaceContext WithConstraint(string theConstraintExpression)
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