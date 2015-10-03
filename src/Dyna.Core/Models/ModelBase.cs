using System;
using System.Diagnostics;
using System.Threading;

namespace Dyna.Core.Models
{
    [Serializable]
    public abstract class ModelBase
    {
        private int id;
        private static int nextIdentity = 1;

        /// <summary>
        /// Gets the unique identifier for the model.
        /// </summary>
        public virtual int Id
        {
            get { return this.id; }
            set
            {
                if (value == default(int))
                    throw new ArgumentException("Id must have a non-default value.",
                                                "value");
                this.id = value;
            }
        }

        /// <summary>
        /// Gets whether the model has an identity.
        /// </summary>
        public bool HasIdentity
        {
            get
            {
                return this.id != default(int);
            }
        }

        /// <summary>
        /// Assign an identity to the model.
        /// </summary>
        public void AssignIdentity()
        {
            Debug.Assert(!this.HasIdentity);

            this.Id = nextIdentity;
            Interlocked.Increment(ref nextIdentity);

            Debug.Assert(this.HasIdentity);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is 
        /// equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the 
        /// current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(Object obj)
        {
            var rhs = obj as ModelBase;

            if (rhs == null) return false;

            return this.Id == rhs.Id;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
