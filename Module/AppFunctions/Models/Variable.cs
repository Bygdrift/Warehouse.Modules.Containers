namespace Module.AppFunctions.Models
{
    public class Variable
    {
        public Variable(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public Variable(string name, string value, string secureValue)
        {
            Name = name;
            Value = value;
            SecureValue = secureValue;
        }

        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string SecureValue { get; set; } = string.Empty;

    }
}