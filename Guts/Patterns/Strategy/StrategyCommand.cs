namespace Guts.Patterns.Strategy
{
    public class StrategyCommand
    {
        public StrategyCommand(string type)
        {
            Type = type;
        }

        public void SetVariable(string name)
        {
            VariableName = name;
            HasVariable = true;
        }

        public void SetVariableValue(object value)
        {
            VariableValue = value;
        }

        public void SetSeparator(bool isSeparator)
        {
            IsSeparator = isSeparator;
        }

        public string Type { get; }
        public string VariableName { get; private set; }
        public object VariableValue { get; private set; }
        public bool HasVariable { get; private set; }
        public bool IsSeparator { get; private set; }
    }
}