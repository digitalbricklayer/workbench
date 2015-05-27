using System;

namespace DynaApp.Entities
{
    class Variable
    {
        public Variable(string theName)
        {
            if (string.IsNullOrWhiteSpace(theName))
                throw new ArgumentException("theName");
            this.Name = theName;
        }

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

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
