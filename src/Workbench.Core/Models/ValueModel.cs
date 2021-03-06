﻿using System;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A value that is bound to a variable.
    /// </summary>
    [Serializable]
    public sealed class ValueModel
    {
        /// <summary>
        /// Initialize a value binding with a model value.
        /// </summary>
        /// <param name="theModelValue">Model value.</param>
        public ValueModel(object theModelValue)
        {
            Model = theModelValue;
        }

        public override string ToString()
        {
            return Model.ToString();
        }

        /// <summary>
        /// Gets the model value.
        /// </summary>
        public object Model { get; private set; }
    }
}
