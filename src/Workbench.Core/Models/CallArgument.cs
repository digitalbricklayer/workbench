namespace Workbench.Core.Models
{
    public class CallArgument
    {
        public CallArgument(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public string Value { get; private set; }
    }
}