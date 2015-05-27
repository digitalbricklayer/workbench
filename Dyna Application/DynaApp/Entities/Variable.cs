using System;

namespace DynaApp.Entities
{
    class Variable
    {
        /// <summary>
        /// Gets or sets the variable name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        public Domain Domain { get; internal set; }

        /// <summary>
        /// Gets or sets the model the variable is a part of.
        /// </summary>
        public Model Model { get; set; }

        public Variable(string theName)
        {
            if (string.IsNullOrWhiteSpace(theName))
                throw new ArgumentException("theName");
            this.Name = theName;
        }
    }
}
