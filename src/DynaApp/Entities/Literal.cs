using System;

namespace DynaApp.Entities
{
    public class Literal
    {
        public Literal(string newLiteral)
        {
            this.Value = Convert.ToInt32(newLiteral);
        }

        public int Value { get; set; }
    }
}