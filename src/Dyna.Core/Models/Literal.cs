using System;

namespace Dyna.Core.Models
{
    [Serializable]
    public class Literal
    {
        public Literal(string newLiteral)
        {
            this.Value = Convert.ToInt32(newLiteral);
        }

        public int Value { get; set; }
    }
}