namespace DynaApp.Entities
{
    class VariableValue
    {
        public int? Value { get; set; }

        public override bool Equals(object obj)
        {
            return this.Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}